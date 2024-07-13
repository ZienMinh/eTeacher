using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class RequirementListModel : PageModel
    {
        private readonly IRequirementService _requirementService;
        private readonly IAuthService _authService;
        private readonly ILogger<RequirementListModel> _logger;

        public RequirementListModel(IRequirementService requirementService,
            IAuthService authService, ILogger<RequirementListModel> logger)
        {
            _requirementService = requirementService;
            _authService = authService;
            _logger = logger;
        }

        [BindProperty]
        public RequirementDto requirementDto { get; set; }

        public List<Requirement> Requirements { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var userId = _authService.GetCurrentUserId();
                if (userId != null)
                {
                    var response = await _requirementService.GetByUserIdAsync(requirementDto, userId);
                    if (response != null)
                    {
                        Requirements = response.Requirements.OrderBy(r => r.Status).ToList();
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
