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
        Task<UserServiceResponseDto> GetAll(UserDto userDto);

        Task<UserServiceResponseDto> GetByIdAsync(string id);

        Task<List<UserDto>> SearchTutorAsync(string name, string subjectName);
    }
}
