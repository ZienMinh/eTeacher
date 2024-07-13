using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
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
    public class ClassHourService : IClassHourService
    {
        private readonly AddDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ClassHourService> _logger;

        public ClassHourService(AddDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClassHourService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ClassHourServiceResponseDto> GetAll(ClassHourDto classHourDto)
        {
            var dsClassHour = await _context.ClassHours.ToListAsync();

            var response = new ClassHourServiceResponseDto
            {
                Classes = dsClassHour
            };

            return response;
        }

        public async Task<ClassHourServiceResponseDto> GetByIdAsync(ClassHourDto classHourDto, string id)
        {
            var classes = await _context.ClassHours.FirstOrDefaultAsync(c => c.Class_id == id);

            if (classes != null)
            {
                return new ClassHourServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Class found.",
                    Classes = new List<ClassHour> { classes }
                };
            }
            else
            {
                return new ClassHourServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No class found for the given ID.",
                    Classes = null
                };
            }
        }

        public async Task<ClassHourServiceResponseDto> GetByUserId(ClassHourDto classHourDto, string id)
        {
            var classes = await _context.ClassHours.Where(c => c.User_id == id).OrderBy(c =>c.Status).ToListAsync();

            if (classes.Any())
            {
                return new ClassHourServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Class found.",
                    Classes = classes
                };
            }
            else
            {
                return new ClassHourServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No class found for the given ID.",
                    Classes = null
                };
            }
        }

        public async Task<List<ClassHour>> SearchSubjectAsync(string subjectName)
        {
            var query = _context.ClassHours
                .Where(u => u.Subject_name == subjectName);

            if (!string.IsNullOrEmpty(subjectName))
            {
                query = query.Where(r => r.Subject.Subject_name.Contains(subjectName));
            }

            var classes = await query.ToListAsync();

            return classes;
        }


        public async Task<ClassHourServiceResponseDto> CreateClassAsync(ClassHourDto model, string userId)
        {
            _logger.LogInformation("Mapping ClassDto to Requirement entity");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var classId = GenerateClassHourId();

                    var classes = new ClassHour
                    {
                        Class_id = classId,
                        Address = model.Address,
                        User_id = model.User_id,
                        Subject_name = model.Subject_name,
                        Start_date = model.Start_date,
                        End_date = model.End_date,
                        Start_time = model.Start_time,
                        End_time = model.End_time,
                        Grade = model.Grade,
                        Price = model.Price,
                        Number_of_session = model.Number_of_session,
                        Description = model.Description,
                        Total = model.Total,
                        Status = model.Status,
                        Link_meet = model.Link_meet,
                    };

                    await _context.AddAsync(classes);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    _logger.LogInformation("Class saved successfully");
                    return new ClassHourServiceResponseDto
                    {
                        IsSucceed = true,
                        CreatedClass = classes,
                        Message = "Class created successfully"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ClassHourServiceResponseDto
                    {
                        IsSucceed = false,
                        Message = ex.Message
                    };
                }
            }
        }

        public async Task<ClassHourServiceResponseDto> UpdateClassHourAsync(ClassHourDto classHourDto)
        {
            var response = new ClassHourServiceResponseDto();

            try
            {
                _logger.LogInformation("Retrieving class hour from database");

                var classHour = await _context.ClassHours.FindAsync(classHourDto.Class_id);

                if (classHour == null)
                {
                    _logger.LogWarning($"Class hour with id {classHourDto.Class_id} not found");
                    response.IsSucceed = false;
                    response.Message = "Class hour not found";
                    return response;
                }

                _logger.LogInformation("Updating class hour entity");

                classHour.Address = classHourDto.Address ?? classHour.Address;
                classHour.Subject_name = classHourDto.Subject_name ?? classHour.Subject_name;
                classHour.Start_date = classHourDto.Start_date ?? classHour.Start_date;
                classHour.End_date = classHourDto.End_date ?? classHour.End_date;
                classHour.Start_time = classHourDto.Start_time ?? classHour.Start_time;
                classHour.End_time = classHourDto.End_time ?? classHour.End_time;
                classHour.Grade = classHourDto.Grade != default(byte) ? classHourDto.Grade : classHour.Grade;
                classHour.Price = classHourDto.Price != default(double) ? classHourDto.Price : classHour.Price;
                classHour.Number_of_session = classHourDto.Number_of_session != default(int) ? classHourDto.Number_of_session : classHour.Number_of_session;
                classHour.Description = classHourDto.Description ?? classHour.Description;
                classHour.Total = classHourDto.Total != null ? classHourDto.Total : classHour.Total;
                classHour.Status = classHourDto.Status != null ? classHourDto.Status : classHour.Status;
                classHour.Link_meet = classHourDto.Link_meet ?? classHour.Link_meet;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Class hour updated successfully");
                response.IsSucceed = true;
                response.Message = "Class hour updated successfully";
                response.CreatedClass = classHour;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating the class hour: " + ex.Message);
                response.IsSucceed = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public async Task<ClassHourServiceResponseDto> DeleteClassAsync(string id)
        {
            var response = new ClassHourServiceResponseDto();

            try
            {
                var classes = await _context.ClassHours.SingleOrDefaultAsync(lo => lo.Class_id == id);
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


        public string GenerateClassHourId()
        {
            int currentCount = _context.ClassHours.Count();
            return "H" + (currentCount + 1).ToString("D9");
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
