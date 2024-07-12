using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class DeleteClassHourModel : PageModel
    {
        private readonly AddDbContext _context;
        private readonly IAuthService _authService;
        private readonly IClassHourService _classHourService;

        public DeleteClassHourModel(AddDbContext context, IAuthService authService, IClassHourService classHourService)
        {
            _context = context;
            _authService = authService;
            _classHourService = classHourService;
        }

        public ClassHour ClassHour { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassHourId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Requirement ID is not provided.");
            }

            ClassHourId = id;

            ClassHour = await _context.ClassHours
                .FirstOrDefaultAsync(r => r.Class_id == id);

            if (ClassHour == null)
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

                var classHourDto = new ClassHourDto
                {
                    Class_id = ClassHourId,
                    Status = 2
                };

                var updateResult = await _classHourService.UpdateClassHourAsync(classHourDto);
                if (updateResult.IsSucceed)
                {
                    return RedirectToPage("/TutorPage");

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
