using Curso.Application.DTOs;
using Curso.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Interfaces;
public interface IStudentService
{
    Task<StudentDTO> CreateAsync(StudentForRegistrationDTO student);
    Task<StudentDTO?> GetAsync(int id, ClaimsPrincipal loggedInUser);
    Task<PagedResult<StudentDTO>> GetAllAsync(QueryParameters queryPara);
    Task<bool> DeleteAsync(int id);
}
