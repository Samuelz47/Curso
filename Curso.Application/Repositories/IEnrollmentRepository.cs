using Curso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Repositories;
public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<bool> IsUniqueAsync(int studentId, int courseId);
}
