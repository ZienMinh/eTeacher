using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_eTeacherSystem.Pages
{
    public class AcceptClassModel : PageModel
    {
        private readonly IRequirementService _requirementService;
        private readonly IClassService _classService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<AcceptClassModel> _logger;

        public AcceptClassModel(IRequirementService requirementService, IClassService classService, 
            IAuthService authService, AddDbContext context, ILogger<AcceptClassModel> logger)
        {
            _requirementService = requirementService;
            _classService = classService;
            _authService = authService;
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        public List<Requirement> Requirements { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var response = await _requirementService.GetAll(new RequirementDto());
                Requirements = response.Requirements.OrderBy(r => r.Status).ToList();
                var userId = _authService.GetCurrentUserId();
                if (userId != null)
                {
                    ClassDto = new ClassDto { Tutor_id = userId };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching classes in OnGetAsync");
            }
        }

    }
}
