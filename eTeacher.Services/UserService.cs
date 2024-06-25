using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly AddDbContext _context;
        private readonly ILogger<RequirementService> _logger;

        public UserService(AddDbContext context, ILogger<RequirementService> logger)
        {
            _context = context;
            _logger = logger;
        }
		public async Task<UserServiceResponseDto> GetAll(UserDto userDto)
        {
            var dsUser = await _context.Users.ToListAsync();

            var response = new UserServiceResponseDto
            {
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

		

	}
}
