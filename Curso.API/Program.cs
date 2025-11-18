using Curso.Application.Interfaces;
using Curso.Application.Mappings;
using Curso.Application.Repositories;
using Curso.Application.Services;
using Curso.Infrastructure.Context;
using Microsoft.Extensions.Logging;
// using Curso.Infrastructure.Context; // Removido
using Curso.Infrastructure.Identity;
using Curso.Infrastructure.Persistence;
using Curso.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
// using Microsoft.OpenApi.Models; // <-- NÃO PRECISA MAIS
using NSwag; // <-- USANDO NSWAG
using NSwag.Generation.Processors.Security; // <-- USANDO NSWAG
using System.Text;
using Curso.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new ArgumentNullException(nameof(jwtKey), "A chave JWT 'Jwt:Key' não foi encontrada.");
}

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAutoMapper(typeof(CourseMappingProfile).Assembly);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- SUBSTITUIÇÃO DO SWAGGER ---
// 1. Substitui o AddSwaggerGen
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "Sistema Ensino API";
    settings.Version = "v1";

    // Configuração do "Authorize" (Bearer Token) para NSwag
    settings.AddSecurity("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Insira seu token JWT desta forma: Bearer {seu_token}",
        In = OpenApiSecurityApiKeyLocation.Header,
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Aplica o cadeado de segurança em todos os endpoints
    settings.OperationProcessors.Add(new OperationSecurityScopeProcessor("Bearer"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DataSeeder.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Um erro ocorreu durante o seeding do banco.");
    }
}

if (app.Environment.IsDevelopment())
{
    // 2. Substitui o UseSwagger()
    app.UseOpenApi();

    // 3. Substitui o UseSwaggerUI()
    app.UseSwaggerUi(); // (Note que é 'Ui' minúsculo)
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();