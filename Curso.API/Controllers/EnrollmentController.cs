using Curso.Application.DTOs;
using Curso.Application.Interfaces;
using Curso.Application.Services;
using Curso.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    [HttpGet("students/{studentId}/enrollments")]
    [Authorize(Roles = "Admin, Student")]
    public async Task<IActionResult> GetEnrollmentByStudent(int studentId, [FromQuery] QueryParameters queryParameters)
    {
        var pagedResult = await _enrollmentService.GetEnrolmmentByStudent(queryParameters, User, studentId);

        var paginationMetadata = new
        {
            pagedResult.TotalCount,
            pagedResult.PageSize,
            pagedResult.PageNumber,
            pagedResult.TotalPages,
            pagedResult.HasNextPage,
            pagedResult.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        return Ok(pagedResult.Items);
    }
}
