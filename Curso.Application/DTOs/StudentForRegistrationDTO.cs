using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.DTOs;
public class StudentForRegistrationDTO
{
    public string FullName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}
