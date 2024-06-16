using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AddDbContext context, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
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
                Message = token
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

            await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User is now an ADMIN"
            };
        }

        public async Task<AuthServiceResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

            if (user == null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid User name!"
                };

            await _userManager.AddToRoleAsync(user, StaticUserRoles.OWNER);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User is now an OWNER"
            };
        }

        public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);

            if (isExistsUser != null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "UserName Already Exists"
                };

            User newUser = new User()
            {
                First_name = registerDto.FirstName,
                Last_name = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
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

			// Add a Default USER Role to all users
			var role = registerDto.Role.ToUpper();
			if (role != StaticUserRoles.USER && role != StaticUserRoles.ADMIN && role != StaticUserRoles.OWNER)
			{
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Invalid Role"
				};
			}

			await _userManager.AddToRoleAsync(newUser, role);

			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "User Created Successfully"
			};
		}   

        public async Task<AuthServiceResponseDto> SeedRolesAsync()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = true,
                    Message = "Roles Seeding is Already Done"
                };

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));

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

        public async Task<AuthServiceResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByNameAsync(resetPasswordDto.UserName);

            if (user == null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "User not found"
                };
            }

            var isOtpValid = await VerifyOtpAsync(user, resetPasswordDto.Otp);
            if (!isOtpValid)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid OTP"
                };
            }

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetPasswordToken, resetPasswordDto.NewPassword);

            if (!resetPasswordResult.Succeeded)
            {
                var errorString = "Password Reset Failed Because: ";
                foreach (var error in resetPasswordResult.Errors)
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
                Message = "Password Reset Successfully"
            };
        }

        private async Task<bool> VerifyOtpAsync(User user, string otp)
        {
            var otpRecord = await _context.Otps
                .Where(o => o.User_id == user.Id && o.Otp_code == otp && o.Expiry_time > DateTime.Now)
                .FirstOrDefaultAsync();

            if (otpRecord != null)
            {
                // Mark OTP as used
                otpRecord.Expiry_time = DateTime.Now;
                _context.Otps.Update(otpRecord);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        Task<bool> IAuthService.VerifyOtpAsync(User user, string otp)
        {
            throw new NotImplementedException();
        }

        public Task SendOtpAsync(User user)
        {
            throw new NotImplementedException();
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
    }
}
