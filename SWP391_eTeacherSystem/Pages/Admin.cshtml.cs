using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{

    public class AdminModel : PageModel
    {
        private readonly IVisitorCounterService _visitorCounterService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;

        public int VisitorCount { get; private set; }
        public int RegisteredUserCount { get; private set; }


        public AdminModel(IAuthService authService, IVisitorCounterService visitorCounterService,AddDbContext context)
        {
            _context = context;
            _authService = authService;
            _visitorCounterService = visitorCounterService;
        }

        public int UserCount { get; set; }
        public void OnGet()
        {
            VisitorCount = _visitorCounterService.GetVisitorCount();
            RegisteredUserCount = _visitorCounterService.GetRegisteredUserCount();
        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _authService.LogoutAsync();
            return RedirectToPage("/Index");
        }
    }
}
