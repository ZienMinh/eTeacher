using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class ClassDetailsModel : PageModel
    {
        private readonly AddDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IClassService _classService;
        private readonly IClassHourService _classHourService;

        public ClassDetailsModel(AddDbContext context, IAuthService authService,
            IUserService userService, IClassService classService, IClassHourService classHourService)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
            _classService = classService;
            _classHourService = classHourService;
        }

        public UserDto UserDto { get; set; }

        public ClassHour ClassHour { get; set; }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassId { get; set; }

        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var classId = _classService.GenerateClassId();
                ClassDto = new ClassDto
                {
                    Tutor_id = ClassHour.User_id,
                    Student_id = userId,
                    Class_id = classId
                };
            }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Class ID is not provided.");
            }

            ClassId = id;

            ClassHour = await _context.ClassHours.FirstOrDefaultAsync(c => c.Class_id == id);

            if (ClassHour == null)
            {
                return NotFound();
            }

            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                ClassDto = new ClassDto
                {
                    Tutor_id = ClassHour.User_id,
                    Student_id = userId
                };
            }

            await InitializeClassDtoAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("ClassId");


            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            ClassDto.Student_id = userId;

            try
            {

                var result = await _classService.CreateClassAsync(ClassDto, userId);
                if (result.IsSucceed)
                {

                    var classHourDto = new ClassHourDto
                    {
                        Class_id = ClassId,
                        Status = 2
                    };

                    var updateResult = await _classHourService.UpdateClassHourAsync(classHourDto);

                    if (updateResult.IsSucceed)
                    {
                        return RedirectToPage("/StudentPage", new { id = result.CreatedClass.Class_id });
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