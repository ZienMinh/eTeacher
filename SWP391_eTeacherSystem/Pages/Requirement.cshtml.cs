using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Linq;
using System.Security.Claims;

namespace SWP391_eTeacherSystem.Pages
{
    public class RequirementModel : PageModel
    {

        private readonly AddDbContext _context;
        private readonly IRequirementService _requirementService;
        private readonly IAuthService _authService;

        private const double PricePerHour = 100000;

        public RequirementModel(AddDbContext context, IAuthService authService, IRequirementService requirementService)
        {
            _context = context;
            _authService = authService;
            _requirementService = requirementService;
        }

        [BindProperty]
        public RequirementDto RequirementDto { get; set; }

        public List<Subject> Subjects { get; set; }


        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var requirementId = _requirementService.GenerateRequirementId();
                RequirementDto = new RequirementDto
                {
                    User_id = userId,
                    Requirement_id = requirementId,
                    Price = PricePerHour
                };
            }
        }


        public async Task OnGetAsync()
        {
            Subjects = await _context.Subjects.ToListAsync();
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                RequirementDto = new RequirementDto { User_id = userId };
            }
            await InitializeClassDtoAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            RequirementDto.User_id = userId;

            RequirementDto.Price = PricePerHour;
            var startTime = RequirementDto.Start_time.Value;
            var endTime = RequirementDto.End_time.Value;
            var duration = (endTime - startTime).TotalHours;
            RequirementDto.Total = RequirementDto.Number_of_session * duration * PricePerHour;


            try
            {
                var result = await _requirementService.CreateRequirementAsync(RequirementDto, userId);
                if (result.IsSucceed)
                {
                    return RedirectToPage("/StudentPage", new { id = result.CreatedRequirement.Requirement_id });
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

            await OnGetAsync();
            return Page();
        }


    }
}