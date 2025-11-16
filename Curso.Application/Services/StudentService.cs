using AutoMapper;
using Curso.Application.DTOs;
using Curso.Application.Interfaces;
using Curso.Application.Repositories;
using Curso.Domain.Entities;
using Curso.Shared.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Services;
public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _identityService;

    public StudentService(IStudentRepository studentRepository, IMapper mapper, IUnitOfWork uow, IIdentityService identityService)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
        _uow = uow;
        _identityService = identityService;
    }

    public async Task<StudentDTO> CreateAsync(StudentForRegistrationDTO student)
    {
        var result = await _identityService.RegisterUserAsync(student.Email, student.Password, "Student");
        if (!result.Succeeded) { throw new ApplicationException("Falha ao criar usuário: " + string.Join(", ", result.Errors)); }

        var createdStudent = new Student(student.FullName, DateTime.UtcNow, result.UserId, false);
        _studentRepository.Create(createdStudent);
        await _uow.CommitAsync();

        var studentDto = _mapper.Map<StudentDTO>(createdStudent);
        return studentDto;
    }

    public async Task<StudentDTO?> GetAsync(int id, ClaimsPrincipal loggedInUser)
    {
        var student = await _studentRepository.GetByIdWithDetailsAsync(id);
        if (student == null) { throw new ApplicationException("Estudante não encontrado"); }

        bool isAdmin = loggedInUser.IsInRole("Admin");
        if (!isAdmin) { return null; }

        string loggedInUserId = loggedInUser.FindFirstValue(ClaimTypes.NameIdentifier);
        bool isOwner = (student.ApplicationUserId == loggedInUserId);
        if (!isOwner) { return null; }

        var studentDto = _mapper.Map<StudentDTO>(student);
        return studentDto;
    }

    public async Task<PagedResult<StudentDTO>> GetAllAsync(QueryParameters queryPara)
    {
        var pagedResultFromRepo = await _studentRepository.GetAllWithDetailsAsync(queryPara);

        var studentsDto = _mapper.Map<IEnumerable<StudentDTO>>(pagedResultFromRepo.Items);

        return new PagedResult<StudentDTO>(
            studentsDto,
            pagedResultFromRepo.TotalCount,
            pagedResultFromRepo.PageNumber,
            pagedResultFromRepo.PageSize
        );
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _studentRepository.GetAsync(s => s.Id == id);
        if (student == null) { return false; }
        student.Delete();
        await _uow.CommitAsync();
        return true;
    }
}
