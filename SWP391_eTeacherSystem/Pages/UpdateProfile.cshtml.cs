using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class UpdateProfileModel : PageModel
    {
        private readonly AddDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UpdateProfileModel(AddDbContext context, IAuthService authService, IUserService userService)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
        }

        public User User { get; set; }

        [BindProperty]
        public UserDto UserDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
            }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("User ID is not provided.");
            }

            UserId = id;

            User = await _context.Users
                .FirstOrDefaultAsync(r => r.Id == id);

            if (User == null)
            {
                return NotFound();
            }
            var userId = _authService.GetCurrentUserId();
            await InitializeClassDtoAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            var updateResult = await _userService.UpdateUserAsync(UserDto);
            if (updateResult.IsSucceed)
            {
                return RedirectToPage("/TutorPage");
            }
            else
            {
                ModelState.AddModelError(string.Empty, updateResult.Message);
            }
            return Page();
        }
    }
}
