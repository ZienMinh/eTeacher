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
        Task<ClassServiceResponseDto> CreateClassAsync(ClassDto model, string userId);

        Task<ClassServiceResponseDto> AcceptClassAsync(ClassDto model);

        Task<ClassServiceResponseDto> DeleteClassAsync(string id);

        string GenerateClassId();

        string GetCurrentUserId();
    }
}
