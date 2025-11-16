using Curso.Application.DTOs;
using Curso.Application.Interfaces;
using Curso.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Curso.API.Controllers;
[Route("api")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Instructor")]
    public async Task<IActionResult> CreateCourse([FromBody] CourseForRegistrationDTO courseForRegistration)
    {
        var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (applicationUserId == null) { return Unauthorized("Token inválido ou vazio"); }

        var courseDto = await _courseService.CreateAsync(courseForRegistration, applicationUserId);

        return CreatedAtAction(nameof(GetCourseById), new { id = courseDto.Id }, courseDto);
    }

    [HttpGet("{id}", Name = "GetCourse")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        var existingCourse = await _courseService.GetByIdAsync(id);
        if (existingCourse is null) { return NotFound("Nenhum curso encontrado"); }

        return Ok(existingCourse);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses([FromQuery] QueryParameters queryParameters)
    {
        var pagedResult = await _courseService.GetAllAsync(queryParameters);

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

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Instructor")]
    public async Task<ActionResult<CourseDTO>> UpdateCourse(int id, CourseForUpdateDTO courseDto)
    {
        var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var updatedCourse = await _courseService.UpdateAsync(id, courseDto, applicationUserId);
        return Ok(updatedCourse);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Instructor")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var result = await _courseService.DeleteAsync(id);

        if (!result)
        {
            return NotFound($"Curso com ID {id} não encontrado.");
        }

        return NoContent();
    }
}
