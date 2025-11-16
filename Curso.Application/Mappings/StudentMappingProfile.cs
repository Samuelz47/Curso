using AutoMapper;
using Curso.Application.DTOs;
using Curso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Mappings;
public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        CreateMap<StudentForRegistrationDTO, Student>();

        CreateMap<Student, StudentDTO>()
            .ForMember(
                dest => dest.EnrolledCourses,
                opt => opt.MapFrom(src => src.Enrollments.Select(e => e.Course.Title)
            )
        );
    }
}
