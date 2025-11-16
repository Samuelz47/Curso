using Curso.Application.DTOs;
using Curso.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Interfaces;
public interface ICourseService
{
    Task<CourseDTO> CreateAsync(CourseForRegistrationDTO courseForRegistration, string applicationUserId);
    Task<CourseDTO?> GetByIdAsync(int id);
    Task<PagedResult<CourseDTO>> GetAllAsync(QueryParameters queryParameters);
    Task<CourseDTO?> UpdateAsync(int id, CourseForUpdateDTO courseDto, string applicationUserId);
    Task<bool> DeleteAsync(int id);
}
