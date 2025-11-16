using Curso.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Curso.Application.Interfaces
{
    public class RegistrationResponse
    {
        public bool Succeeded { get; set; }
        public string UserId { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }

    public interface IIdentityService
    {
        Task<RegistrationResponse> RegisterUserAsync(string email, string password, string role);
        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto);
    }
}