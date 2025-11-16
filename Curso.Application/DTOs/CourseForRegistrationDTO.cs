using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.DTOs;
public class CourseForRegistrationDTO
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [MinLength(3, ErrorMessage = "O título deve ter no mínimo 3 caracteres.")]
    public string Title { get; set; }

    public string Description { get; set; }

    public string Category { get; set; }

    public int CourseLoad { get; set; }
}
