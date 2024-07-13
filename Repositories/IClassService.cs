using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IClassService
    {
        Task<ClassServiceResponseDto> GetByIdAsync(ClassDto classDto, string id);

        Task<ClassServiceResponseDto> GetByStudentIdAsync(ClassDto classDto, string id);

        Task<ClassServiceResponseDto> GetByTutorIdAsync(ClassDto classDto, string id);

        Task<ClassServiceResponseDto> CreateClassAsync(ClassDto model, string userId);

        Task<ClassServiceResponseDto> DeleteClassAsync(string id);

        Task<ClassServiceResponseDto> UpdateClassAsync(ClassDto classDto);


        string GenerateClassId();
        string GetCurrentUserId();
    }
}