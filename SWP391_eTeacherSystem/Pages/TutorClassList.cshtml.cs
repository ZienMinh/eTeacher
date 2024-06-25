using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class TutorClassListModel : PageModel
    {
        private readonly IClassService _classService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<TutorClassListModel> _logger;

        public TutorClassListModel(IClassService classService, IAuthService authService,
            AddDbContext context, ILogger<TutorClassListModel> logger)
        {
            _classService = classService;
            _authService = authService;
            _context = context;
            _logger = logger;
            ClassDto = new ClassDto();
        }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        public List<Class> Classes { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                _logger.LogInformation($"Fetching classes for tutor with ID: {userId}");
                var response = await _classService.GetByTutorIdAsync(ClassDto, userId);
                if (response.IsSucceed)
                {
                    Classes = response.Classes;
                    _logger.LogInformation("Classes successfully fetched");
                }
                else
                {
                    Classes = new List<Class>();
                    _logger.LogWarning(response.Message);
                }
            }
            else
            {
                _logger.LogWarning("User is not authenticated");
                Classes = new List<Class>();
            }
        }


    }
}
