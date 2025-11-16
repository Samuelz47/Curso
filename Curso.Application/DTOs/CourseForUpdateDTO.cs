using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.DTOs;
public class CourseForUpdateDTO
{
    public string Description { get; set; }
    public string Category { get; set; }
    public int CourseLoad { get; set; }
}
