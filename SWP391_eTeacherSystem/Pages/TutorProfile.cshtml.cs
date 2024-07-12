using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using System.Linq;
using System.Security.Claims;

namespace SWP391_eTeacherSystem.Pages
{
    public class TutorProfileModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<TutorProfileModel> _logger;
        private readonly IUserService _userService;

        public TutorProfileModel(IUserService userService, IAuthService authService, AddDbContext context, ILogger<TutorProfileModel> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        [BindProperty]
        public UserDto UserDto { get; set; }
        public QualificationDto QualificationDto { get; set; }  

        public User User { get; set; }
        public Qualification Qualification { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var userId = _authService.GetCurrentUserId();
                if (userId != null)
                {
                    var userResponse = await _userService.GetByIdAsync(userId);
                    if (userResponse.IsSucceed)
                    {
                        User = userResponse.Users.First();
                    }

                    var qualificationResponse = await _userService.GetQualificationByIdAsync(QualificationDto, userId);
                    if (qualificationResponse.IsSucceed)
                    {
                        Qualification = qualificationResponse.Qualifications.First();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user profile");
            }

        }
    }
}
