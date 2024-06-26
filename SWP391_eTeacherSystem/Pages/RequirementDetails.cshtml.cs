using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class RequirementDetailsModel : PageModel
    {
        private readonly AddDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ILogger<RequirementDetailsModel> _logger;

        public Requirement Requirement { get; set; }
        public UserDto CurrentUser { get; set; }

        public RequirementDetailsModel(AddDbContext context, IAuthService authService, IUserService userService, ILogger<RequirementDetailsModel> logger)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
            _logger = logger;
        }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            _logger.LogInformation("OnGetAsync started with ID: {ID}", id);

            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Requirement ID is not provided.");
                return NotFound("Requirement ID is not provided.");
            }

            Requirement = await _context.Requirements
                .FirstOrDefaultAsync(r => r.Requirement_id == id);

            if (Requirement == null)
            {
                _logger.LogWarning("Requirement not found.");
                return NotFound();
            }

            /*var userId = _authService.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User is not authenticated.");
                return Unauthorized();
            }

            CurrentUser = await _userService.GetByIdAsync(userId);
            if (CurrentUser == null)
            {
                _logger.LogWarning("User not found.");
                return NotFound("User not found.");
            }*/

            _logger.LogInformation("OnGetAsync completed successfully.");
            return Page();
        }
    }
}
