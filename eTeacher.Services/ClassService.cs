using BusinessObject.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace Services
{

    public class ClassService : IClassService
    {
        private readonly AddDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RequirementService> _logger;

        public ClassService(AddDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<RequirementService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ClassServiceResponseDto> CreateClassAsync(ClassDto model, string userId)
        {
            _logger.LogInformation("Mapping ClassDto to Requirement entity");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (string.IsNullOrEmpty(userId))
                    {
                        return new ClassServiceResponseDto
                        {
                            IsSucceed = false,
                            Message = "User is not authenticated."
                        };
                    }

                    var subject = await _context.Subjects.SingleOrDefaultAsync(s => s.Subject_name == model.Subject_name);
                    if (subject == null)
                    {
                        return new ClassServiceResponseDto
                        {
                            IsSucceed = false,
                            Message = "Invalid Subject_name."
                        };
                    }

                    var classId = GenerateClassId();

                    var classes = new Class
                    {
                        Class_id = classId,
                        Address = model.Address,
                        Student_id = model.Student_id,  // Cho phép null
                        Tutor_id = userId,
                        Subject_name = model.Subject_name,
                        Start_date = model.Start_date,
                        End_date = model.End_date,
                        Start_time = model.Start_time,
                        End_time = model.End_time,
                        Grade = model.Grade,
                        Type_class = model.Type_class,
                        Price = model.Price,
                        Number_of_session = model.Number_of_session
                    };

                    await _context.AddAsync(classes);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    _logger.LogInformation("Class saved successfully");
                    return new ClassServiceResponseDto
                    {
                        IsSucceed = true,
                        CreatedClass = classes,
                        Message = "Class created successfully"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ClassServiceResponseDto
                    {
                        IsSucceed = false,
                        Message = ex.Message
                    };
                }
            }
        }



        public async Task<ClassServiceResponseDto> AcceptClassAsync(ClassDto model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userId = GetCurrentUserId();
                    if (userId == null)
                    {
                        return new ClassServiceResponseDto
                        {
                            IsSucceed = false,
                            Message = "User is not authenticated."
                        };
                    }

                    var subject = await _context.Subjects.SingleOrDefaultAsync(s => s.Subject_name == model.Subject_name);
                    if (subject == null)
                    {
                        return new ClassServiceResponseDto
                        {
                            IsSucceed = false,
                            Message = "Invalid Subject_name."
                        };
                    }

                    var requirement = await _context.Requirements.SingleOrDefaultAsync(r => r.User_id == model.Student_id);
                    if (requirement == null)
                    {
                        return new ClassServiceResponseDto
                        {
                            IsSucceed = false,
                            Message = "Invalid Requirement."
                        };
                    }

                    var classId = GenerateClassId();

                    var classes = new Class
                    {
                        Class_id = classId,
                        Address = model.Address,
                        Student_id = requirement.User_id,
                        Tutor_id = userId,
                        Subject_name = requirement.Subject_name,
                        Start_date = requirement.Start_date,
                        End_date = requirement.End_date,
                        Start_time = requirement.Start_time,
                        End_time = requirement.End_time,
                        Grade = requirement.Grade,
                        Type_class = 1,
                        Price = requirement.Price,
                        Number_of_session = requirement.Number_of_session
                    };

                    _context.Add(classes);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ClassServiceResponseDto
                    {
                        IsSucceed = true,
                        Message = "Class created successfully"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ClassServiceResponseDto
                    {
                        IsSucceed = false,
                        Message = ex.Message
                    };
                }
            }
        }

        public async Task<ClassServiceResponseDto> DeleteClassAsync(string id)
        {
            var response = new ClassServiceResponseDto();

            try
            {
                var classes = await _context.Classes.SingleOrDefaultAsync(lo => lo.Class_id == id);
                if (classes != null)
                {
                    _context.Remove(classes);
                    await _context.SaveChangesAsync();
                    response.IsSucceed = true;
                    response.Message = "Class deleted successfully";
                    response.CreatedClass = classes;
                }
                else
                {
                    response.IsSucceed = false;
                    response.Message = "Class not found";
                }
            }
            catch (Exception ex)
            {
                response.IsSucceed = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public string GenerateClassId()
        {
            int currentCount = _context.Classes.Count();
            return "C" + (currentCount + 1).ToString("D9");
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
