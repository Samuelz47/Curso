using AutoMapper;
using Curso.Application.DTOs;
using Curso.Application.Interfaces;
using Curso.Application.Repositories;
using Curso.Domain.Entities;
using Curso.Domain.Enums;
using Curso.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Application.Services;
public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public EnrollmentService(IEnrollmentRepository enrollmentRepository, IMapper mapper, IUnitOfWork uow, ICourseRepository courseRepository, IStudentRepository studentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
        _mapper = mapper;
        _uow = uow;
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
    }

    public async Task<EnrollmentDTO> CreateAsync(EnrollmentForRegistrationDTO enrollmentForRegisterDto, ClaimsPrincipal loggedInUser)
    {
        bool isAdmin = loggedInUser.IsInRole("Admin");
        int studentIdToEnroll;

        if (isAdmin)
        {
            if (!enrollmentForRegisterDto.StudentId.HasValue)
            {
                throw new ApplicationException("Admin deve fornecer o 'studentId' para matricular um aluno.");
            }
            studentIdToEnroll = enrollmentForRegisterDto.StudentId.Value;
        }
        else
        {
            if (enrollmentForRegisterDto.StudentId.HasValue)
            {
                throw new ApplicationException("Estudante não pode matricular outros alunos.");
            }
            string loggedInApplicationUserId = loggedInUser.FindFirstValue(ClaimTypes.NameIdentifier);

            var studentProfile = await _studentRepository.GetAsync(s => s.ApplicationUserId == loggedInApplicationUserId);
            if (studentProfile == null)
            {
                throw new ApplicationException("Perfil de estudante não encontrado para o usuário logado.");
            }
            studentIdToEnroll = studentProfile.Id;
        }

        var existingCourse = await _courseRepository.GetAsync(c => c.Id == enrollmentForRegisterDto.CourseId);
        var existingStudent = await _studentRepository.GetAsync(s => s.Id == studentIdToEnroll);

        if (existingCourse is null || existingStudent is null || existingStudent.IsDeleted) { throw new ApplicationException("Curso ou estudante não encontrado"); }

        var enrollment = new Enrollment(existingCourse.Id, studentIdToEnroll, Status.Active);
        bool isUnique = await _enrollmentRepository.IsUniqueAsync(studentIdToEnroll, existingCourse.Id);
        if (!isUnique) { throw new ApplicationException("Matrícula duplicada. Este aluno já está neste curso."); }
        enrollment.Course = existingCourse;
        enrollment.Student = existingStudent;

        _enrollmentRepository.Create(enrollment);
        await _uow.CommitAsync();

        var enrollmentDto = _mapper.Map<EnrollmentDTO>(enrollment);
        return enrollmentDto;
    }
        
    public async Task<PagedResult<EnrollmentDTO>> GetEnrolmmentByStudent(QueryParameters queryP, ClaimsPrincipal loggedInUser, int studentId)
    {
        var student = await _studentRepository.GetAsync(s => s.Id == studentId);
        if (student == null) throw new ApplicationException("Estudante não encontrado");

        bool isAdmin = loggedInUser.IsInRole("Admin");
        string loggedInUserId = loggedInUser.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!isAdmin && student.ApplicationUserId != loggedInUserId)
        {
            throw new UnauthorizedAccessException("Sem permissão.");
        }

        var pagedResult = await _enrollmentRepository.GetEnrollmentsByStudent(studentId, queryP);

        var dtos = _mapper.Map<List<EnrollmentDTO>>(pagedResult.Items);

        return new PagedResult<EnrollmentDTO>(
            dtos,
            pagedResult.TotalCount,
            pagedResult.PageNumber,
            pagedResult.PageSize
        );
    }
}
