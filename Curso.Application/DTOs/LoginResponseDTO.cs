using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.DTOs;
public class LoginResponseDTO
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string Email { get; set; }
}
