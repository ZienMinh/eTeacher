using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RequirementService : IRequirementService
    {
        private readonly AddDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RequirementService> _logger;

        public RequirementService(AddDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<RequirementService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<(bool IsSucceed, string Message, Requirement CreatedRequirement)> CreateRequirementAsync(RequirementDto requirementDto, string userId)
        {
            try
            {
                _logger.LogInformation("Mapping RequirementDto to Requirement entity");

                var requirement = new Requirement
                {
                    Requirement_id = requirementDto.Requirement_id,
                    User_id = requirementDto.User_id,
                    Subject_name = requirementDto.Subject_name,
                    Start_date = requirementDto.Start_date,
                    End_date = requirementDto.End_date,
                    Start_time = requirementDto.Start_time,
                    End_time = requirementDto.End_time,
                    Grade = requirementDto.Grade,
                    Rank = requirementDto.Rank,
                    Price = requirementDto.Price,
                    Number_of_session = requirementDto.Number_of_session,
                    Address = requirementDto.Address,
                    Description = requirementDto.Description,
                    Total = requirementDto.Total,
                    Status = requirementDto.Status,

                };

                _context.Requirements.Add(requirement);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Requirement saved successfully");
                return (true, "Requirement created successfully", requirement);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while saving the requirement: " + ex.Message);
                return (false, ex.Message, null);
            }
        }

        public async Task<RequirementServiceResponseDto> GetAll(RequirementDto requirementDto)
        {
            var dsRequirement = await _context.Requirements.ToListAsync();

            var response = new RequirementServiceResponseDto
            {
                Requirements = dsRequirement
            };

            return response;
        }

        public async Task<RequirementServiceResponseDto> GetByIdAsync(RequirementDto requirementDto, string id)
        {
            var requirement = await _context.Requirements
                                            .FirstOrDefaultAsync(r => r.Requirement_id == id);

            if (requirement != null)
            {
                return new RequirementServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Requirement found.",
                    Requirements = new List<Requirement> { requirement }
                };
            }
            else
            {
                return new RequirementServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No requirement found for the given ID.",
                    Requirements = null
                };
            }
        }


        public async Task<RequirementServiceResponseDto> GetByUserIdAsync(RequirementDto requirementDto, string id)
        {
            var requirement = await _context.Requirements
                                        .Where(c => c.User_id == id)
                                        .OrderBy(c => c.Status)
                                        .ToListAsync();

            if (requirement.Any())
            {
                return new RequirementServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Classes found.",
                    Requirements = requirement
                };
            }
            else
            {
                return new RequirementServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No class found for the given ID.",
                    Requirements = null
                };
            }
        }

        public async Task<RequirementServiceResponseDto> UpdateRequirementAsync(RequirementDto requirementDto)
        {
            var response = new RequirementServiceResponseDto();

            try
            {
                _logger.LogInformation("Retrieving requirement from database");

                // Lấy requirement từ database
                var requirement = await _context.Requirements.FindAsync(requirementDto.Requirement_id);

                if (requirement == null)
                {
                    _logger.LogWarning($"Requirement with id {requirementDto.Requirement_id} not found");
                    response.IsSucceed = false;
                    response.Message = "Requirement not found";
                    return response;
                }

                // Cập nhật các thuộc tính của requirement nếu có giá trị mới
                _logger.LogInformation("Updating requirement entity");

                requirement.Subject_name = requirementDto.Subject_name ?? requirement.Subject_name;
                requirement.Start_date = requirementDto.Start_date ?? requirement.Start_date;
                requirement.End_date = requirementDto.End_date ?? requirement.End_date;
                requirement.Start_time = requirementDto.Start_time ?? requirement.Start_time;
                requirement.End_time = requirementDto.End_time ?? requirement.End_time;
                requirement.Grade = requirementDto.Grade != default(byte) ? requirementDto.Grade : requirement.Grade;
                requirement.Rank = requirementDto.Rank ?? requirement.Rank;
                requirement.Price = requirementDto.Price != default(double) ? requirementDto.Price : requirement.Price;
                requirement.Number_of_session = requirementDto.Number_of_session != default(int) ? requirementDto.Number_of_session : requirement.Number_of_session;
                requirement.Address = requirementDto.Address ?? requirement.Address;
                requirement.Description = requirementDto.Description ?? requirement.Description;
                requirement.Total = requirementDto.Total != null ? requirementDto.Total : requirement.Total;
                requirement.Status = requirementDto.Status != null ? requirementDto.Status : requirement.Status;

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();

                _logger.LogInformation("Requirement updated successfully");
                response.IsSucceed = true;
                response.Message = "Requirement updated successfully";
                response.CreatedRequirement = requirement;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating the requirement: " + ex.Message);
                response.IsSucceed = false;
                response.Message = ex.Message;
            }

            return response;
        }




        public async Task<RequirementServiceResponseDto> DeleteByIdAsync(string id)
        {
            var requirement = await _context.Requirements.SingleOrDefaultAsync(lo => lo.Requirement_id == id);
            if (requirement != null)
            {
                _context.Remove(requirement);
                await _context.SaveChangesAsync();

                return new RequirementServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Requirement deleted successfully."
                };
            }
            else
            {
                return new RequirementServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "Requirement not found."
                };
            }
        }


        public string GenerateRequirementId()
        {
            int currentCount = _context.Requirements.Count();
            return "R" + (currentCount + 1).ToString("D9");
        }

        public string GetCurrentUserId()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        }

    }
}
