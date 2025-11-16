using Curso.Application.Repositories;
using Curso.Domain.Entities;
using Curso.Infrastructure.Context;
using Curso.Shared.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Infrastructure.Repositories;
public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context) { }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses
            .Include(c => c.Instructor)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<PagedResult<Course>> GetAllAsync(QueryParameters queryParameters)
    {
        var query  = _context.Courses.Include(c => c.Instructor)
                                     .AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                               .Take(queryParameters.PageSize)
                               .ToListAsync();

        return new PagedResult<Course>(
            items,
            totalCount,
            queryParameters.PageNumber,
            queryParameters.PageSize
        );
    }
}
