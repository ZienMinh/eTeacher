using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class ClassListModel : PageModel
    {
        private readonly IClassService _classService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<ClassListModel> _logger;

        public ClassListModel(IClassService classService, IAuthService authService, AddDbContext context, ILogger<ClassListModel> logger)
        {
            _classService = classService;
            _authService = authService;
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        public List<Class> Classes { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var userId = _authService.GetCurrentUserId();
                var response = await _classService.GetByStudentIdAsync(ClassDto, userId);
                Classes = response.Classes;
                if (userId == null)
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
