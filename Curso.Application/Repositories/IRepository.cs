using Curso.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Repositories;
public interface IRepository<T>
{
    Task<PagedResult<T>> GetAllAsync(QueryParameters queryParameters);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}
