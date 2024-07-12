﻿using System;
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
    }
}
