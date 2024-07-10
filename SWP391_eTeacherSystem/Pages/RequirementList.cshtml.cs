using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class RequirementListModel : PageModel
    {
        private readonly IRequirementService _requirementService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<RequirementListModel> _logger;

        public RequirementListModel(IRequirementService requirementService,
            IAuthService authService, AddDbContext context, ILogger<RequirementListModel> logger)
        {
            _requirementService = requirementService;
            _authService = authService;
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public RequirementDto requirementDto { get; set; }

        public List<Requirement> Requirements { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var userId = _authService.GetCurrentUserId();
                if (userId != null)
                {
                    var response = await _requirementService.GetByUserIdAsync(requirementDto, userId);
                    if (response != null)
                    {
                        Requirements = response.Requirements.OrderBy(r => r.Status).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching classes in OnGetAsync");
            }
        }

        public async Task<IActionResult> OnPostAsync(string RequirementId)
        {
            try
            {
                var userId = _authService.GetCurrentUserId();
                if (userId == null)
                {
                    ModelState.AddModelError(string.Empty, "User is not authenticated.");
                    return Page();
                }

                var requimentDto = new RequirementDto
                {
                    Requirement_id = RequirementId,
                    Status = 2
                };

                var updateResult = await _requirementService.UpdateRequirementAsync(requimentDto);
                if (updateResult.IsSucceed)
                {
                    return RedirectToPage("/StudentPage");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, updateResult.Message);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
            }
            return Page();
        }
    }
}
