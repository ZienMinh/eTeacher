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

        public Requirement Requirement { get; set; }
        public UserDto UserDto { get; set; }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RequirementId { get; set; }

        public RequirementDetailsModel(AddDbContext context, IAuthService authService, IUserService userService, 
            IClassService classService, IRequirementService requirementService)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
            _classService = classService;
            _requirementService = requirementService;
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
            if (userId != null)
            {
                ClassDto = new ClassDto { Tutor_id = userId };
            }

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

            ClassDto.Tutor_id = userId;

            try
            {
                var result = await _classService.CreateClassAsync(ClassDto, userId);
                if (result.IsSucceed)
                {

                    var requimentDto = new RequirementDto
                    {
                        Requirement_id = RequirementId,
                        Status = 2
                    };

                    var updateResult = await _requirementService.UpdateRequirementAsync(requimentDto);
                    if (updateResult.IsSucceed)
                    {
                        return RedirectToPage("/TutorPage", new { id = result.CreatedClass.Class_id });

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, updateResult.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
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
