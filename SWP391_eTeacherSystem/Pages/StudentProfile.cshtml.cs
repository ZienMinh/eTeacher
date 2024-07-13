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
    public class StudentProfileModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<StudentProfileModel> _logger;
        private readonly IUserService _userService;

        public StudentProfileModel(IUserService userService, IAuthService authService, AddDbContext context, ILogger<StudentProfileModel> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        [BindProperty]
        public UserDto UserDto { get; set; }

        public User User { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var userId = _authService.GetCurrentUserId();
                if (userId != null)
                {
                    var response = await _userService.GetByIdAsync(userId);
                    if (response.IsSucceed && response.Users != null && response.Users.Any())
                    {
                        User = response.Users.First();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user profile");
            }
        }
    }
}
