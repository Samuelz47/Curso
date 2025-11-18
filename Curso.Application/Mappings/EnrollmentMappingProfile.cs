using AutoMapper;
using Curso.Application.DTOs;
using Curso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Mappings;
public class EnrollmentMappingProfile : Profile
{
    public EnrollmentMappingProfile()
    {
        CreateMap<EnrollmentForRegistrationDTO, Enrollment>();

        CreateMap<Enrollment, EnrollmentDTO>()
            .ForMember(
                dest => dest.CourseName,
                opt => opt.MapFrom(src => src.Course.Title)
            )
            .ForMember(
                dest => dest.StudentName,
                opt => opt.MapFrom(src => src.Student.FullName)
            );
    }
}
