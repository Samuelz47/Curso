using Curso.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Domain.Entities;
public class Enrollment
{
    public Enrollment(int courseId, int studentId, Status status)
    {
        CourseId = courseId;
        StudentId = studentId;
        Status = Status.Active;
    }

    public int Id { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public Status Status { get; set; }
}
