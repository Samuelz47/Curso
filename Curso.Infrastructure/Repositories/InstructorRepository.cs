using Curso.Application.Repositories;
using Curso.Domain.Entities;
using Curso.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Infrastructure.Repositories;
public class InstructorRepository : Repository<Instructor>, IInstructorRepository
{
    public InstructorRepository(AppDbContext context) : base(context) { }
}
