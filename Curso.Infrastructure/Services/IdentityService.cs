using Curso.Application.DTOs;
using Curso.Application.Interfaces;
using Curso.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Infrastructure.Services;
public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager = null, IConfiguration configuration = null)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<RegistrationResponse> RegisterUserAsync(string email, string password, string role)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return new RegistrationResponse
            {
                Succeeded = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        await _userManager.AddToRoleAsync(user, role);

        return new RegistrationResponse
        {
            Succeeded = true,
            UserId = user.Id
        };
    }

    public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            throw new ApplicationException("Usuário ou senha inválidos.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            throw new ApplicationException("Usuário ou senha inválidos.");
        }

        var tokenResult = await GenerateJwtToken(user);

        return new LoginResponseDTO
        {
            Token = tokenResult.Token,
            Expiration = tokenResult.Expiration,
            Email = user.Email
        };
    }

    private async Task<(string Token, DateTime Expiration)> GenerateJwtToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id), 
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenString, expiration);
    }
}
