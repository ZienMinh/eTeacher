﻿using BusinessObject.Models;
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

        public AcceptClassModel(IRequirementService requirementService, IClassService classService, 
            IAuthService authService, AddDbContext context)
        {
            _requirementService = requirementService;
            _classService = classService;
            _authService = authService;
            _context = context;
        }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        public List<Requirement> Requirements { get; set; }

        public async Task OnGetAsync()
        {
            var response = await _requirementService.GetAll(new RequirementDto());
            Requirements = response.Requirements;
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                ClassDto = new ClassDto { Tutor_id = userId };
            }
        }

    }
}
