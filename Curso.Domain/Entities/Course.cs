using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Domain.Entities;
public class Course
{
    public Course(string title, string description, string category, int courseLoad, DateTime creationDate, int instructorId)
    {
        Title = title;
        Description = description;
        Category = category;
        CourseLoad = courseLoad;
        CreationDate = creationDate;
        InstructorId = instructorId;
    }

    public int Id { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public int CourseLoad { get; set; }
    public DateTime CreationDate { get; set; }
    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; }
    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
