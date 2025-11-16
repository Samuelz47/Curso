using AutoMapper;
using Curso.Application.DTOs;
using Curso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Mappings;
public class CourseMappingProfile : Profile
{
    public CourseMappingProfile()
    {
        // Mapeamento de "Criação" (DTO -> Entidade)
        CreateMap<CourseForRegistrationDTO, Course>();
        CreateMap<CourseForUpdateDTO, Course>();

        // Mapeamento de "Resposta" (Entidade -> DTO)
        CreateMap<Course, CourseDTO>()
            .ForMember(
                dest => dest.InstructorName,
                opt => opt.MapFrom(src => src.Instructor.FullName)
            );
    }
}
