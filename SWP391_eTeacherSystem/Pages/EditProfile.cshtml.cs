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
	public class EditProfileModel : PageModel
	{
		private readonly AddDbContext _context;
		private readonly IUserService _userService;
		private readonly ILogger<EditProfileModel> _logger;

		public EditProfileModel(AddDbContext context,IUserService userService, ILogger<EditProfileModel> logger)
		{
			_context = context;
			_userService = userService;
			_logger = logger;
		}

		[BindProperty]
		public UserDto UserDto { get; set; }


		public async Task OnGetAsync()
		{
			var userId = _userService.GetCurrentUserId();
		}
         
		public async Task<IActionResult> OnPostAsync()
		{
			_logger.LogInformation("OnPostAsync started");

			if (!ModelState.IsValid)
			{
				_logger.LogWarning("ModelState is not valid");
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

			var userId = _userService.GetCurrentUserId();
			if (userId == null)
			{
				_logger.LogWarning("User is not authenticated");
				ModelState.AddModelError(string.Empty, "User is not authenticated.");
				return Page();
			}

			try
			{
				_logger.LogInformation("Attempting to edit profile");
				var result = await _userService.UpdateUserAsync(UserDto);
				if (result.IsSucceed)
				{
					_logger.LogInformation("Edit profile successfully");
					return RedirectToPage("/TutorProfile", new { });
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
