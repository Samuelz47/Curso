using Curso.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Interfaces;
public interface IEnrollmentService
{
    Task<EnrollmentDTO> CreateAsync(EnrollmentForRegistrationDTO enrollmentDto, ClaimsPrincipal loggedInUser);
}
