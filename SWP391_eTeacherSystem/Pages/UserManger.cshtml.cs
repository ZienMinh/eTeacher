using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class UserManagerModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<UserManagerModel> _logger;
        private readonly IUserService _userService;

        public UserManagerModel(IUserService userService, IAuthService authService, AddDbContext context, ILogger<UserManagerModel> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        [BindProperty]
        public UserDto UserDto { get; set; }

        public List<UserDto> Users { get; set; } = new List<UserDto>();

        public async Task OnGetAsync()
        {
            var response = await _userService.GetAllAsync();
            if (response != null && response.IsSucceed && response.Users != null)
            {
                Users = response.Users.Select(user => new UserDto
                {
                    First_name = user.First_name,
                    Last_name = user.Last_name,
                    Gender = user.Gender,
                    Email = user.Email,
                    Link_contact = user.Link_contact,
                    Rating = user.Rating,
                    Role = user.Role
                })
                    .OrderBy(user => user.Role)
                    .ToList();
            }
            else
            {
                _logger.LogWarning(response?.Message ?? "Failed to load users.");
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            _logger.LogInformation($"Attempting to delete user with ID: {id}");

            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("User ID is null or empty.");
                return Page();
            }

            var response = await _userService.DeleteUserAsync(id);

            if (response.IsSucceed)
            {
                _logger.LogInformation("User deleted successfully.");
            }
            else
            {
                _logger.LogWarning(response.Message);
            }

            // Refresh the user list
            await OnGetAsync();

            return Page();
        }

    }
}
