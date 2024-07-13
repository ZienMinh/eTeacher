using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class ClassHourListModel : PageModel
    {
        private readonly IClassHourService _classHourService;
        private readonly IAuthService _authService;
        private readonly ILogger<ClassHourListModel> _logger;

        public ClassHourListModel(IClassHourService classHourService, IAuthService authService, ILogger<ClassHourListModel> logger)
        {
            _classHourService = classHourService;
            _authService = authService;
            _logger = logger;
        }

        [BindProperty]
        public ClassHourDto classHourDto { get; set; }

        public List<ClassHour> ClassHours { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var userId = _authService.GetCurrentUserId();
                if (userId != null)
                {
                    var response = await _classHourService.GetByUserId(classHourDto, userId);
                    if (response != null)
                    {
                        ClassHours = response.Classes.OrderBy(c => c.Status).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching classes in OnGetAsync");
            }
        }
    }
}
