using Curso.Application.DTOs;
using Curso.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Curso.API.Controllers;
[Route("[controller]")]
[ApiController]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Student")]
    public async Task<ActionResult> CreateEnrollment(EnrollmentForRegistrationDTO enrollmentForReg)
    {
        var enrollmentDto = await _enrollmentService.CreateAsync(enrollmentForReg, User);
        return Ok(enrollmentDto);
    }
}
