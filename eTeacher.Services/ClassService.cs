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


        public async Task<ClassServiceResponseDto> GetByIdAsync(ClassDto classDto, string id)
        {
            var classes = await _context.Classes.FirstOrDefaultAsync(c => c.Class_id == id);

            if (classes != null)
            {
                return new ClassServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Class found.",
                    Classes = new List<Class> { classes }
                };
            }
            else
            {
                return new ClassServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No class found for the given ID.",
                    Classes = null
                };
            }
        }

        public async Task<ClassServiceResponseDto> GetByStudentIdAsync(ClassDto classDto, string id)
        {
            var classes = await _context.Classes
                                        .Where(c => c.Student_id == id)
                                        .OrderBy(c => c.Status)
                                        .ToListAsync();

            if (classes.Any())
            {
                return new ClassServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Classes found.",
                    Classes = classes
                };
            }
            else
            {
                return new ClassServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No class found for the given ID.",
                    Classes = null
                };
            }
        }



        public async Task<ClassServiceResponseDto> GetByTutorIdAsync(ClassDto classDto, string id)
        {
            var classes = await _context.Classes.Where(c => c.Tutor_id == id).OrderBy(c => c.Status).ToListAsync();

            if (classes.Any())
            {
                return new ClassServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Classes found.",
                    Classes = classes
                };
            }
            else
            {
                return new ClassServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No class found for the given ID.",
                    Classes = null
                };
            }
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
                        Student_id = model.Student_id,
                        Tutor_id = model.Tutor_id,
                        Subject_name = model.Subject_name,
                        Start_date = model.Start_date,
                        End_date = model.End_date,
                        Start_time = model.Start_time,
                        End_time = model.End_time,
                        Grade = model.Grade,
                        Type_class = model.Type_class,
                        Price = model.Price,
                        Number_of_session = model.Number_of_session,
                        Total = model.Total,
                        Status = model.Status,
                        Link_meet = model.Link_meet,
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


        public async Task<ClassServiceResponseDto> UpdateClassAsync(ClassDto classDto)
        {
            var response = new ClassServiceResponseDto();

            try
            {
                _logger.LogInformation("Retrieving class from database");

                var classes = await _context.Classes.FindAsync(classDto.Class_id);

                if (classes == null)
                {
                    _logger.LogWarning($"Class with id {classDto.Class_id} not found");
                    response.IsSucceed = false;
                    response.Message = "Class not found";
                    return response;
                }

                _logger.LogInformation("Updating class hour entity");

                classes.Address = classDto.Address ?? classes.Address;
                classes.Subject_name = classDto.Subject_name ?? classes.Subject_name;
                classes.Start_date = classDto.Start_date ?? classes.Start_date;
                classes.End_date = classDto.End_date ?? classes.End_date;
                classes.Start_time = classDto.Start_time ?? classes.Start_time;
                classes.End_time = classDto.End_time ?? classes.End_time;
                classes.Grade = classDto.Grade != default(byte) ? classDto.Grade : classes.Grade;
                classes.Price = classDto.Price != default(double) ? classDto.Price : classes.Price;
                classes.Number_of_session = classDto.Number_of_session != default(int) ? classDto.Number_of_session : classes.Number_of_session;
                classes.Total = classDto.Total != null ? classDto.Total : classes.Total;
                classes.Status = classDto.Status != null ? classDto.Status : classes.Status;
                classes.Link_meet = classDto.Link_meet ?? classes.Link_meet;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Class updated successfully");
                response.IsSucceed = true;
                response.Message = "Class updated successfully";
                response.CreatedClass = classes;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating the class: " + ex.Message);
                response.IsSucceed = false;
                response.Message = ex.Message;
            }

            return response;
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
            var maxClassId = _context.Classes
                .OrderByDescending(c => c.Class_id)
                .Select(c => c.Class_id)
                .FirstOrDefault();

            int currentCount = 0;

            if (maxClassId != null && maxClassId.StartsWith("C") && int.TryParse(maxClassId.Substring(1), out int parsedId))
            {
                currentCount = parsedId;
            }

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
