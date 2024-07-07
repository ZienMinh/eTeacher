using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace eTeacher.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AddDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, 
            AddDbContext context, IEmailService emailService, IHttpContextAccessor httpContextAccessor, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials"
                };

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordCorrect)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials"
                };

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("JWTID", Guid.NewGuid().ToString()),
                    new Claim("FirstName", user.First_name),
                    new Claim("LastName", user.Last_name),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = token,
                Role = user.Role
            };
        }


        public async Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
		{
			var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

			if (user == null)
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Invalid User name!"
				};

			await _userManager.AddToRoleAsync(user, StaticUserRoles.GetRoleName(StaticUserRoles.ADMIN));

			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "User is now an ADMIN"
			};
		}

		public async Task<AuthServiceResponseDto> MakeTutorAsync(UpdatePermissionDto updatePermissionDto)
		{
			var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

			if (user == null)
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Invalid User name!"
				};

			await _userManager.AddToRoleAsync(user, nameof(StaticUserRoles.TUTOR));

			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "User is now an TUTOR"
			};
		}

		private async Task<string> GetNormalizedRoleAsync(string roleName)
		{
			var role = await _roleManager.FindByNameAsync(roleName);
			return role?.NormalizedName;
		}

        public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);

            if (isExistsUser != null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "UserName Already Exists"
                };
            }

            var role = registerDto.Role;

            User newUser = new User()
            {
                First_name = registerDto.FirstName,
                Last_name = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                Gender = registerDto.Gender,
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = role,
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Because: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorString
                };
            }

            var roleName = StaticUserRoles.GetRoleName(role);
            var roleResult = await _userManager.AddToRoleAsync(newUser, roleName);

            if (!roleResult.Succeeded)
            {
                var errorString = "Role Assignment Failed Because: ";
                foreach (var error in roleResult.Errors)
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
                Message = "User Created Successfully"
            };
        }


        public async Task<AuthServiceResponseDto> SeedRolesAsync()
		{
            bool isModeratorRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.GetRoleName(StaticUserRoles.MODERATOR));
            bool isTutorRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.GetRoleName(StaticUserRoles.TUTOR));
			bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.GetRoleName(StaticUserRoles.ADMIN));
			bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.GetRoleName(StaticUserRoles.USER));

			if (isModeratorRoleExists && isTutorRoleExists && isAdminRoleExists && isUserRoleExists)
				return new AuthServiceResponseDto()
				{
					IsSucceed = true,
					Message = "Roles Seeding is Already Done"
				};

			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.GetRoleName(StaticUserRoles.USER)));
			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.GetRoleName(StaticUserRoles.ADMIN)));
			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.GetRoleName(StaticUserRoles.TUTOR)));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.GetRoleName(StaticUserRoles.MODERATOR)));

            return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "Role Seeding Done Successfully"
			};
		}

		private string GenerateNewJsonWebToken(List<Claim> claims)
		{
			var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

			var tokenObject = new JwtSecurityToken(
					issuer: _configuration["JWT:ValidIssuer"],
					audience: _configuration["JWT:ValidAudience"],
					expires: DateTime.Now.AddHours(1),
					claims: claims,
					signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
				);

			string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

			return token;
		}

        public async Task<AuthServiceResponseDto> ResetPasswordByEmailAsync(string userName)
        {
            _logger.LogInformation("ResetPasswordByEmailAsync started");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserName}", userName);
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "User not found"
                };
            }

            var newPassword = GenerateRandomPassword();

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordChangeResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (!passwordChangeResult.Succeeded)
            {
                var errorString = "Password update failed because: ";
                foreach (var error in passwordChangeResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                _logger.LogError("Password update failed: {Errors}", errorString);
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorString
                };
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorString = "Failed to update user in database: ";
                foreach (var error in result.Errors)
                {
                    errorString += " # " + error.Description;
                }
                _logger.LogError("Database update failed: {Errors}", errorString);
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorString
                };
            }

            // Send email to user with new password
            var emailSubject = "Your Password Has Been Reset";
            var emailBody = $"Hello {user.UserName},\n\nYour password has been reset. Your new password is: {newPassword}\n\nPlease change it after logging in.";
            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

            _logger.LogInformation("Password reset successfully and email sent to {Email}", user.Email);
            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "Password reset successfully and email sent"
            };
        }

        public string GenerateRandomPassword()
        {
            const int passwordLength = 9;
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            var password = new char[passwordLength];

            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }

            return new string(password);
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

            user.First_name = userDto.First_name;
            user.Last_name = userDto.Last_name;
            user.Email = userDto.Email;
            user.Gender = userDto.Gender;
            user.Address = userDto.Address;
            user.Birth_date = userDto.Birth_date;
            user.Link_contact = userDto.Link_contact;
            user.Image = userDto.Image;

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


        public Task RegisterAsyn(RegisterDto registerDto)
        {
            throw new NotImplementedException();
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

        public async Task<AuthServiceResponseDto> LogoutAsync()
        {
            // Xóa token khỏi session hoặc cookie
            _httpContextAccessor.HttpContext.Session.Remove("AccessToken");

            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "Logout successful"
            };
        }
    }
}
