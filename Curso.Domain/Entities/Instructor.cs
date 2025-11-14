using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Domain.Entities;
public class Instructor
{
    public Instructor(string fullName, string applicationUserId)
    {
        FullName = fullName;
        ApplicationUserId = applicationUserId;
    }

    public int Id { get; set; }
    [Required]
    public string FullName { get; set; }
    public string ApplicationUserId { get; private set; }
    public List<Course> Courses { get; set; } = new List<Course>();
}
