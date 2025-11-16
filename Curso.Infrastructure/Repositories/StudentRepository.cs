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
public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<Student?>> GetAllWithDetailsAsync(QueryParameters queryPara)
    {
        var query = _context.Students.Include(s => s.Enrollments)
                                     .ThenInclude(e => e.Course)
                                     .AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query.Skip((queryPara.PageNumber - 1) * queryPara.PageSize)
                               .Take(queryPara.PageSize)
                               .ToListAsync();

        return new PagedResult<Student>(
            items,
            totalCount,
            queryPara.PageNumber,
            queryPara.PageSize
        );
    }

    public async Task<Student?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Students.Include(s => s.Enrollments)
                                      .ThenInclude(e => e.Course)
                                      .FirstOrDefaultAsync(s => s.Id == id);
    }
}
