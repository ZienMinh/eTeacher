using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserService
    {
        Task<UserServiceResponseDto> GetAllAsync();

        Task<UserServiceResponseDto> GetByIdAsync(string id);

        Task<List<UserDto>> SearchTutorAsync(string name);

        Task<UserServiceResponseDto> DeleteUserAsync(string id);

        Task<AuthServiceResponseDto> UpdateUserAsync(UserDto userDto);

        string GetCurrentUserId();

        Task<QualificationServiceResponseDto> CreateQualificationAsync(QualificationDto qualificationDto);

        Task<QualificationServiceResponseDto> UpdateQualificationAsync(QualificationDto qualificationDto);

        Task<QualificationServiceResponseDto> GetQualificationByIdAsync(QualificationDto qualificationDto, string id);

        string GenerateQualificationId();
    }
}
