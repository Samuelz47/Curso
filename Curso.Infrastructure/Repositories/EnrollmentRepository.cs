using Curso.Application.Repositories;
using Curso.Domain.Entities;
using Curso.Domain.Enums;
using Curso.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Infrastructure.Repositories;
public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(AppDbContext context) : base(context) { }

    public async Task<bool> IsUniqueAsync(int studentId, int courseId)
    {
        bool alreadyExists = await _context.Enrollments.AnyAsync(e =>
            e.CourseId == courseId &&
            e.StudentId == studentId &&
            e.Status == Status.Active
        );

        return !alreadyExists;
    }
}
