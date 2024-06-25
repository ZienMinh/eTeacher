using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDto> SeedRolesAsync();
        Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<bool> VerifyOtpAsync(User user, string otp);
        Task SendOtpAsync(User user);
        Task RegisterAsyn(RegisterDto registerDto);

		Task<AuthServiceResponseDto> UpdateUserAsync(UserDto userDto);
		string GetCurrentUserId();
    }
}
