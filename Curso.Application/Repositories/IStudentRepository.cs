using Curso.Domain.Entities;
using Curso.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Repositories;
public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> GetByIdWithDetailsAsync(int id);
    Task<PagedResult<Student?>> GetAllWithDetailsAsync(QueryParameters queryPara);
}
