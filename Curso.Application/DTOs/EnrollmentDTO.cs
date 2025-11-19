using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.DTOs;
public class EnrollmentDTO
{
    public string StudentName { get; set; }
    public int StudentId { get; set; }
    public string CourseName { get; set; }
    public int CourseId { get; set; }
    public string Status { get; set; }
}
