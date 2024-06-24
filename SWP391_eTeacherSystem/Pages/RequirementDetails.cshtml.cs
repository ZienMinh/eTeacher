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
using NuGet.Protocol;

namespace SWP391_eTeacherSystem.Pages
{
    public class RequirementDetailsModel : PageModel
    {
        private readonly AddDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IClassService _classService;
        private readonly IRequirementService _requirementService;
        private readonly ILogger<RequirementDetailsModel> _logger;

        public Requirement Requirement { get; set; }
        public UserDto UserDto { get; set; }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RequirementId { get; set; }

        public RequirementDetailsModel(AddDbContext context, IAuthService authService, IUserService userService, IClassService classService, IRequirementService requirementService, ILogger<RequirementDetailsModel> logger)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
            _classService = classService;
            _requirementService = requirementService;
            _logger = logger;
        }

        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var classId = _classService.GenerateClassId();
                ClassDto = new ClassDto { Tutor_id = userId, Class_id = classId };
            }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            _logger.LogInformation("OnGetAsync started with ID: {ID}", id);

            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Requirement ID is not provided.");
                return NotFound("Requirement ID is not provided.");
            }

            RequirementId = id;

            Requirement = await _context.Requirements
                .FirstOrDefaultAsync(r => r.Requirement_id == id);

            if (Requirement == null)
            {
                _logger.LogWarning("Requirement not found.");
                return NotFound();
            }

            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                ClassDto = new ClassDto { Tutor_id = userId };
            }

            _logger.LogInformation("OnGetAsync completed successfully.");
            await InitializeClassDtoAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("RequirementId");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is not valid");
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        _logger.LogWarning($"Property: {state.Key} - Errors: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
                return Page();
            }

            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogWarning("User is not authenticated");
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            ClassDto.Tutor_id = userId;

            try
            {
                _logger.LogInformation("Attempting to create class and delete requirement");
                var result = await _classService.CreateClassAsync(ClassDto, userId);
                if (result.IsSucceed)
                {
                    // Log the ID to ensure it's being passed correctly
                    _logger.LogInformation("Deleting requirement with ID: {ID}", RequirementId);

                    var delete = await _requirementService.DeleteByIdAsync(RequirementId); // Use the stored ID
                    if (delete.IsSucceed)
                    {
                        _logger.LogInformation("Class created and requirement deleted successfully");
                        return RedirectToPage("/ClassDetails", new { id = result.CreatedClass.Class_id });
                    }
                    else
                    {
                        _logger.LogWarning("Failed to delete requirement: " + delete.Message);
                        ModelState.AddModelError(string.Empty, delete.Message);
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to create class: " + result.Message);
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while saving data: " + ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
            }

            return Page();
        }
    }
}
