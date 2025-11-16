using Curso.Application.DTOs;
using Curso.Application.Interfaces;
using Curso.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Security.Claims;
using System.Text.Json;

namespace Curso.API.Controllers;
[Route("[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("student/{id}", Name = "GetStudent")]
    [Authorize(Roles = "Admin, Student")]
    public async Task<ActionResult<StudentDTO>> GetStudent(int id)
    {
        var existingStudent = await _studentService.GetAsync(id, User);
        if (existingStudent == null) { return Unauthorized(); }

        return Ok(existingStudent);
    }

    [HttpGet("students")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedResult<StudentDTO>>> GetAllStudents([FromQuery] QueryParameters queryPara)
    {
        var pagedResult = await _studentService.GetAllAsync(queryPara);

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

    [HttpDelete("students/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var isDelete = await _studentService.DeleteAsync(id);
        if(!isDelete) { return NotFound(); }
        return NoContent();
    }
}
