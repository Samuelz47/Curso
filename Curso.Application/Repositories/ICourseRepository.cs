using Curso.Domain.Entities;
using Curso.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Repositories;
public interface ICourseRepository : IRepository<Course>
{
    Task<Course?> GetByIdAsync(int id);
    Task<PagedResult<Course>> GetAllAsync(QueryParameters queryParameters);
}
