using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using System.Linq;
using System.Security.Claims;

namespace SWP391_eTeacherSystem.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<ProfileModel> _logger;
        private readonly IUserService _userService;

        public ProfileModel(IUserService userService, IAuthService authService, AddDbContext context, ILogger<ProfileModel> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        [BindProperty]
        public UserDto UserDto { get; set; }

        public List<User> Users { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                // Handle the case where the user is not logged in
                _logger.LogWarning("User not logged in.");
                return;
            }

            var response = await _userService.GetByIdAsync(userId);
            if (response != null && response.IsSucceed && response.Users != null)
            {
                var user = response.Users.FirstOrDefault();
                if (user != null)
                {
                    UserDto = new UserDto
                    {
                        First_name = user.First_name,
                        Last_name = user.Last_name,
                        Gender = user.Gender,
                        Address = user.Address,
                        Email = user.Email,
                        Birth_date = user.Birth_date,
                        Link_contact = user.Link_contact,
                        Rating = user.Rating,
                        Image = user.Image,
                        Role = user.Role,

                    };
                }
                else
                {
                    _logger.LogWarning("User list is empty.");
                }
            }
            else
            {
                _logger.LogWarning(response?.Message ?? $"User with ID {userId} not found.");
            }

        }
    }
}
