using AutoMapper;
using Curso.Application.DTOs;
using Curso.Application.Interfaces;
using Curso.Application.Repositories;
using Curso.Domain.Entities;
using Curso.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Services;
public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IInstructorRepository _instructorRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public CourseService(ICourseRepository courseRepository, IMapper mapper, IUnitOfWork uof, IInstructorRepository instructorRepository)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
        _uow = uof;
        _instructorRepository = instructorRepository;
    }

    public async Task<CourseDTO> CreateAsync(CourseForRegistrationDTO courseForRegistration, string applicationUserId)
    {
        if (courseForRegistration is null) { throw new ArgumentNullException(nameof(courseForRegistration)); }

        var instructor = await _instructorRepository.GetAsync(i => i.ApplicationUserId == applicationUserId);
        if (instructor is null) { throw new ApplicationException("Perfil de instrutor não encontrado para este usuário."); }

        var course = _mapper.Map<Course>(courseForRegistration);
        course.CreationDate = DateTime.UtcNow;
        course.InstructorId = instructor.Id;

        _courseRepository.Create(course);
        await _uow.CommitAsync();

        var courseDto = _mapper.Map<CourseDTO>(course);
        courseDto.InstructorName = instructor.FullName;
        return courseDto;
    }

    public async Task<CourseDTO?> GetByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course is null) { return null; }

        var courseDto = _mapper.Map<CourseDTO>(course);
        return courseDto;
    }

    public async Task<PagedResult<CourseDTO>> GetAllAsync(QueryParameters queryParameters)
    {
        var pagedResultFromRepo = await _courseRepository.GetAllAsync(queryParameters);

        var coursesDto = _mapper.Map<IEnumerable<CourseDTO>>(pagedResultFromRepo.Items);

        return new PagedResult<CourseDTO>(
            coursesDto,
            pagedResultFromRepo.TotalCount,
            pagedResultFromRepo.PageNumber,
            pagedResultFromRepo.PageSize
        );
    }

    public async Task<CourseDTO?> UpdateAsync(int id, CourseForUpdateDTO courseDto, string applicationUserId)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course is null) { return null; }

        _mapper.Map(courseDto, course);
        _courseRepository.Update(course);
        await _uow.CommitAsync();

        var courseDtoResult = _mapper.Map<CourseDTO>(course);
        return courseDtoResult;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _courseRepository.GetAsync(c => c.Id == id);
        if (course is null) { return false; }

        _courseRepository.Delete(course);
        await _uow.CommitAsync();
        return true;
    }
}
