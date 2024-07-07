using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Helpers;
using Services;
using SWP391_eTeacherSystem.Helpers;

namespace SWP391_eTeacherSystem.Pages
{
    public class ClassModel : PageModel
    {
        private readonly IClassHourService _classhourService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private const double PricePerHour = 100000;

        public ClassModel(IClassHourService classhourService, IAuthService authService, AddDbContext context)
        {
            _classhourService = classhourService;
            _authService = authService;
            _context = context;
        }
        [BindProperty]
        public ClassHourDto ClassHourDto { get; set; }

        public List<Subject> Subjects { get; set; }

        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var classId = _classhourService.GenerateClassHourId();
                ClassHourDto = new ClassHourDto {
                    User_id = userId, 
                    Class_id = classId,
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
				ClassHourDto = new ClassHourDto { User_id = userId };
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

			ClassHourDto.User_id = userId;

            ClassHourDto.Price = PricePerHour;
            var startTime = ClassHourDto.Start_time.Value;
            var endTime = ClassHourDto.End_time.Value;
            var duration = (endTime - startTime).TotalHours;
            ClassHourDto.Total = ClassHourDto.Number_of_session * duration * PricePerHour;

            try
            {
                var result = await _classhourService.CreateClassAsync(ClassHourDto, userId);
                if (result.IsSucceed)
                {
                    return RedirectToPage("/TutorPage", new { id = result.CreatedClass.Class_id });
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
