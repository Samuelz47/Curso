using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Domain.Entities;
public class Student
{
    public Student(string fullName, string email, DateTime registerDate, string applicationUserId, bool isDeleted)
    {
        FullName = fullName;
        Email = email;
        RegisterDate = registerDate;
        ApplicationUserId = applicationUserId;
        IsDeleted = false;
    }

    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public DateTime RegisterDate { get; set; }
    public string ApplicationUserId { get; set; }
    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public bool IsDeleted { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void Restore()
    {
        IsDeleted = false;
    }
}
