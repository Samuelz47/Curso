using Curso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.DTOs;
public class StudentDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public DateTime RegisterDate { get; set; }
    public List<string> EnrolledCourses { get; set; } = new List<string>();
}
