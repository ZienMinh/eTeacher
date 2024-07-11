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
        Task<AuthServiceResponseDto> MakeTutorAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> ResetPasswordByEmailAsync(string userName);
        Task RegisterAsyn(RegisterDto registerDto);


        string GetCurrentUserId();
        string GenerateRandomPassword();

        Task<AuthServiceResponseDto> LogoutAsync();
    }
}
