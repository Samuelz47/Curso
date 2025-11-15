using Curso.Domain.Entities;
using Curso.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Infrastructure.Context;
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Instructor>()
                    .HasMany(i => i.Courses)
                    .WithOne(c => c.Instructor)
                    .HasForeignKey(c => c.InstructorId);

        modelBuilder.Entity<Course>()
                    .HasMany(c => c.Enrollments)
                    .WithOne(e => e.Course)
                    .HasForeignKey(e => e.CourseId);

        modelBuilder.Entity<Student>(entity =>
        {
            // Relacionamento
            entity.HasMany(e => e.Enrollments)
                  .WithOne(e => e.Student)
                  .HasForeignKey(e => e.StudentId);

            // Índice de e-mail único
            entity.HasIndex(s => s.Email)
                  .IsUnique();

            // Filtro de Soft Delete
            entity.HasQueryFilter(s => !s.IsDeleted);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            // Evita matrícula duplicada
            entity.HasIndex(e => new { e.StudentId, e.CourseId })
                  .IsUnique();

            // Esconde matrículas se o Aluno associado estiver deletado
            entity.HasQueryFilter(e => !e.Student.IsDeleted);
        });
    }
}
