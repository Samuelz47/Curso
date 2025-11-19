using Curso.Application.Repositories;
using Curso.Domain.Entities;
using Curso.Domain.Enums;
using Curso.Infrastructure.Context;
using Curso.Shared.Common;
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

    public async Task<PagedResult<Enrollment>> GetEnrollmentsByStudent(int studentId, QueryParameters queryP)
    {
        var query = _context.Enrollments.AsNoTracking()
                                        .Include(e => e.Student)
                                        .Include(e => e.Course)
                                        .Where(e => e.StudentId == studentId)
                                        .AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query.Skip((queryP.PageNumber - 1) * queryP.PageSize)
                               .Take(queryP.PageSize)
                               .ToListAsync();

        return new PagedResult<Enrollment>(
            items,
            totalCount,
            queryP.PageNumber,
            queryP.PageSize
        );
    }
}
