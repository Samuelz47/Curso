using Curso.Application.DTOs;
using Curso.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Curso.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IStudentService _studentService;

    public AuthController(IIdentityService identityService, IStudentService studentService)
    {
        _identityService = identityService;
        _studentService = studentService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterStudent([FromBody] StudentForRegistrationDTO registerDto)
    {
        try
        {
            var studentDto = await _studentService.CreateAsync(registerDto);
            
            return Ok(studentDto);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginDto)
    {
        try
        {
            var response = await _identityService.LoginAsync(loginDto);
            return Ok(response);
        }
        catch (ApplicationException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}
