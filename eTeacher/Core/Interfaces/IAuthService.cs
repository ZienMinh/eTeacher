using eTeacher.Core.DTO;
using eTeacher.Data;

namespace eTeacher.Core.Interfaces
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


    }
}
