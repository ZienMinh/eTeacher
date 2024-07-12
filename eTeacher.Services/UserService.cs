using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly AddDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AddDbContext context, ILogger<UserService> logger, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserServiceResponseDto> GetAllAsync()
        {
            var dsUser = await _context.Users.ToListAsync();

            var response = new UserServiceResponseDto
            {
                IsSucceed = true,
                Users = dsUser
            };

            return response;
        }

        public async Task<UserServiceResponseDto> GetByIdAsync(string id)
        {
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                return new UserServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "User found.",
                    Users = new List<User> { user }
                };
            }
            else
            {
                return new UserServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No user found for the given user ID.",
                    Users = null
                };
            }
        }

        public async Task<List<UserDto>> SearchTutorAsync(string name)
        {
            var query = _context.Users
                .Where(u => u.Role == 3);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.First_name.Contains(name) || u.Last_name.Contains(name));
            }

            var tutors = await query.Select(u => new UserDto
            {
                UserName = u.UserName,
                First_name = u.First_name,
                Last_name = u.Last_name,
                Gender = u.Gender,
                Address = u.Address,
                Link_contact = u.Link_contact,
                Rating = u.Rating,
                Image = u.Image,
                Role = u.Role
            }).ToListAsync();

            return tutors;
        }

        public async Task<UserServiceResponseDto> DeleteUserAsync(string id)
        {
            var response = new UserServiceResponseDto();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    response.IsSucceed = false;
                    response.Message = "User not found.";
                    return response;
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                response.IsSucceed = true;
                response.Message = "User deleted successfully.";
            }
            catch (Exception ex)
            {
                response.IsSucceed = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<AuthServiceResponseDto> UpdateUserAsync(UserDto userDto)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "User Not Logged In"
                };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "User Not Found"
                };
            }

            user.First_name = userDto.First_name ?? userDto.First_name;
            user.Last_name = userDto.Last_name ?? userDto.Last_name;
            user.Email = userDto.Email ?? userDto.Email;
            user.Gender = userDto.Gender ?? userDto.Gender;
            user.Address = userDto.Address ?? userDto.Address;
            user.Birth_date = userDto.Birth_date !=default(DateOnly) ? userDto.Birth_date:userDto.Birth_date;
            user.Link_contact = userDto.Link_contact ?? userDto.Link_contact;
            user.PhoneNumber = userDto.PhoneNumber ?? userDto.PhoneNumber;
            user.Image = userDto.Image ?? userDto.Image;
            user.Rating = userDto.Rating != default(byte) ? userDto.Rating : userDto.Rating;
            user.Role = userDto.Role != default(byte) ? userDto.Role : userDto.Role;


            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                var errorString = "User Update Failed Because: ";
                foreach (var error in updateResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorString
                };
            }

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User Updated Successfully"
            };
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

        public async Task<QualificationServiceResponseDto> CreateQualificationAsync(QualificationDto qualificationDto)
        {
            //_logger.LogInformation("Mapping QualificationDto to Requirement entity");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userId = GetCurrentUserId();

                    var qualification = new Qualification
                    {
                        Qualification_id = qualificationDto.Qualification_id,
                        User_id = userId,
                        Classification = qualificationDto.Classification,
                        Graduation_year = qualificationDto.Graduation_year,
                        Image = qualificationDto.Image,
                        Specialize = qualificationDto.Specialize,
                        Training_facility = qualificationDto.Training_facility,
                    };

                    await _context.AddAsync(qualification);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    _logger.LogInformation("Class saved successfully");
                    return new QualificationServiceResponseDto
                    {
                        IsSucceed = true,
                        CreatedQualification = qualification,
                        Message = "Class created successfully"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new QualificationServiceResponseDto
                    {
                        IsSucceed = false,
                        Message = ex.Message
                    };
                }
            }
        }

        public async Task<QualificationServiceResponseDto> UpdateQualificationAsync(QualificationDto qualificationDto)
        {
            var response = new QualificationServiceResponseDto();

            try
            {
                _logger.LogInformation("Retrieving class hour from database");

                var qualification = await _context.Qualifications.FindAsync(qualificationDto.User_id);

                if (qualification == null)
                {
                    _logger.LogWarning($"Class hour with id {qualificationDto.User_id} not found");
                    response.IsSucceed = false;
                    response.Message = "Class hour not found";
                    return response;
                }

                // Cập nhật các thuộc tính của class hour nếu có giá trị mới
                _logger.LogInformation("Updating class hour entity");

                qualification.Graduation_year = qualificationDto.Graduation_year != default(int) ? qualificationDto.Graduation_year : qualificationDto.Graduation_year;
                qualification.Specialize = qualificationDto.Specialize ?? qualificationDto.Specialize;
                qualification.Classification = qualificationDto.Classification ?? qualificationDto.Qualification_id;
                qualification.Image = qualificationDto.Image ?? qualificationDto.Image;
                qualification.Training_facility = qualificationDto.Training_facility ?? qualificationDto.Training_facility;

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();

                _logger.LogInformation("Class hour updated successfully");
                response.IsSucceed = true;
                response.Message = "Class hour updated successfully";
                response.CreatedQualification = qualification;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating the class hour: " + ex.Message);
                response.IsSucceed = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<QualificationServiceResponseDto> GetQualificationByIdAsync(QualificationDto qualificationDto, string id)
        {
            var qualification = await _context.Qualifications
                                            .FirstOrDefaultAsync(r => r.User_id == id);

            if (qualification != null)
            {
                return new QualificationServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Qualification found.",
                    Qualifications = new List<Qualification> { qualification }
                };
            }
            else
            {
                return new QualificationServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No qualification found for the given ID.",
                    Qualifications = null
                };
            }
        }

        public string GenerateQualificationId()
        {
            var maxQuaId = _context.Qualifications
                .OrderByDescending(c => c.Qualification_id)
                .Select(c => c.Qualification_id)
                .FirstOrDefault();

            int currentCount = 0;

            if (maxQuaId != null && maxQuaId.StartsWith("Q") && int.TryParse(maxQuaId.Substring(1), out int parsedId))
            {
                currentCount = parsedId;
            }

            return "Q" + (currentCount + 1).ToString("D9");
        }
    }
}
