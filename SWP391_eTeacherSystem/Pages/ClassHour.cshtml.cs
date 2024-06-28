using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class ClassHourModel : PageModel
    {
        private readonly IClassHourService _classHourService;
        private readonly IClassService _classService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<ClassHourModel> _logger;

        public ClassHourModel(IClassHourService classHourService, IClassService classService,
            IAuthService authService, AddDbContext context, ILogger<ClassHourModel> logger)
        {
            _classHourService = classHourService;
            _classService = classService;
            _authService = authService;
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        public List<ClassHour> Classes { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var response = await _classHourService.GetAll(new ClassHourDto());
                Classes = response.Classes;
                var userId = _authService.GetCurrentUserId();
                if (userId != null)
                {
                    ClassDto = new ClassDto { Student_id = userId };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching classes in OnGetAsync");
            }
        }
    }

}
