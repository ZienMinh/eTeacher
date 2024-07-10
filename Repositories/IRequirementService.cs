using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
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

        Task<RequirementServiceResponseDto> GetAll(RequirementDto requirementDto);

        Task<RequirementServiceResponseDto> GetByIdAsync(RequirementDto requirementDto, string id);

        Task<RequirementServiceResponseDto> GetByUserIdAsync(RequirementDto requirementDto, string id);

        Task<RequirementServiceResponseDto> UpdateRequirementAsync(RequirementDto requirementDto);

        Task<RequirementServiceResponseDto> DeleteByIdAsync(string id);

        string GenerateRequirementId();

        string GetCurrentUserId();
    }
}
