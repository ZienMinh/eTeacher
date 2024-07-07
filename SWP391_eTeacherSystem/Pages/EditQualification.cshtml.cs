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
	public class EditQualificationModel : PageModel
	{
		private readonly AddDbContext _context;
		private readonly IUserService _userService;
		private readonly IAuthService _authService;
		private readonly ILogger<EditQualificationModel> _logger;

		public EditQualificationModel(AddDbContext context, IAuthService authService, IUserService userService, ILogger<EditQualificationModel> logger)
		{
			_context = context;
			_authService = authService;
			_userService = userService;
			_logger = logger;
		}

		[BindProperty]
		public UserDto UserDto { get; set; }


		public async Task OnGetAsync()
		{
			var userId = _authService.GetCurrentUserId();
			
		}
         
		public async Task<IActionResult> OnPostAsync()
		{
			_logger.LogInformation("OnPostAsync started");

			if (!ModelState.IsValid)
			{
				_logger.LogWarning("ModelState is not valid");
				// Log chi tiết các lỗi trong ModelState
				foreach (var state in ModelState)
				{
					if (state.Value.Errors.Count > 0)
					{
						_logger.LogWarning($"Property: {state.Key} - Errors: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
					}
				}
				await OnGetAsync();
				return Page();
			}

			var userId = _authService.GetCurrentUserId();
			if (userId == null)
			{
				_logger.LogWarning("User is not authenticated");
				ModelState.AddModelError(string.Empty, "User is not authenticated.");
				return Page();
			}



			try
			{
				_logger.LogInformation("Attempting to edit profile");
				var result = await _authService.UpdateUserAsync(UserDto);
				if (result.IsSucceed)
				{
					_logger.LogInformation("Edit profile successfully");
					return RedirectToPage("/Profile");
				}
				else
				{
					_logger.LogWarning("Failed to edit profile: " + result.Message);
					ModelState.AddModelError(string.Empty, result.Message);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred while saving data: " + ex.Message);
				ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
			}

			await OnGetAsync();
			return Page();
		}
	}
}
