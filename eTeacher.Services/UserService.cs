using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess;
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

        public async Task<List<UserDto>> SearchTutorAsync(string name, string subjectName)
        {
            var query = _context.Users
                .Include(u => u.Requirements)
                .ThenInclude(r => r.Subject)
                .Where(u => u.Role == 3);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.First_name.Contains(name) || u.Last_name.Contains(name));
            }

            if (!string.IsNullOrEmpty(subjectName))
            {
                query = query.Where(u => u.Requirements.Any(r => r.Subject.Subject_name.Contains(subjectName)));
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
    }
}
