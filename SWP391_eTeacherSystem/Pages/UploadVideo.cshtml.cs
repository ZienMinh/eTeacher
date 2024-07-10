using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using Services.Interfaces;

namespace SWP391_eTeacherSystem.Pages
{
    public class UploadVideoModel : PageModel
    {
        private readonly IAcademicVideoService _videoService;
        private readonly IAuthService _authService;

        public UploadVideoModel(IAcademicVideoService videoService, IAuthService authService)
        {
            _videoService = videoService;
            _authService = authService;
        }

        [BindProperty]
        public IFormFile VideoFile { get; set; }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Description { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = _authService.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            var videoUrl = await _videoService.UploadVideoAsync(VideoFile);
            var video = new AcademicVideo
            {
                Tutor_id = userId,
                Title = Title,
                Description = Description,
                VideoUrl = videoUrl
            };

            await _videoService.AddVideoAsync(video);
            return RedirectToPage("/TutorPage");
        }
    }
}
