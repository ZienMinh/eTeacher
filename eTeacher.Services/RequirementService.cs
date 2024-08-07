﻿using BusinessObject.Models;
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
                    Description = requirementDto.Description
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
