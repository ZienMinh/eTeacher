using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRequirementService
    {
        //Task<RequirementServiceResponseDto> CreateRequirementAsync(RequirementDto model, string userId);
        Task<(bool IsSucceed, string Message, Requirement CreatedRequirement)> CreateRequirementAsync(RequirementDto requirementDto, string userId);

        string GenerateRequirementId();

        string GetCurrentUserId();
    }
}
