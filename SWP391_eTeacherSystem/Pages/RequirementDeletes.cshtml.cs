using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class RequirementDeletesModel : PageModel
    {
        private readonly AddDbContext _context;
        private readonly IAuthService _authService;
        private readonly IRequirementService _requirementService;

        public RequirementDeletesModel(AddDbContext context, IAuthService authService, IRequirementService requirementService)
        {
            _context = context;
            _authService = authService;
            _requirementService = requirementService;
        }

        public Requirement Requirement { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RequirementId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Requirement ID is not provided.");
            }

            RequirementId = id;

            Requirement = await _context.Requirements
                .FirstOrDefaultAsync(r => r.Requirement_id == id);

            if (Requirement == null)
            {
                return NotFound();
            }

            var userId = _authService.GetCurrentUserId();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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
