﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger,IVisitorCounterService visitorCounterService)
        {
            _visitorCounterService = visitorCounterService;
            _logger = logger;
        }
    }
}
