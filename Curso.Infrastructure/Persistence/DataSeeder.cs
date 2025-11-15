using Curso.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Infrastructure.Persistence;
public class DataSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = services.GetRequiredService<ILogger<DataSeeder>>();

        string[] roleNames = { "Admin", "Instructor", "Student" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                logger.LogInformation("Criando role: {roleName}", roleName);
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminEmail = "admin@sistema.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            logger.LogInformation("Criando usuário admin: {adminEmail}", adminEmail);
            var newUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(newUser, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, "Admin");
                logger.LogInformation("Usuário admin criado e adicionado ao role 'Admin'");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    logger.LogError("Erro ao criar admin: {error}", error.Description);
                }
            }
        }
    }
}
