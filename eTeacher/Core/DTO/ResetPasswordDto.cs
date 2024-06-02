using System;
using System.ComponentModel.DataAnnotations;

namespace eTeacher.Core.DTO
{
	public class ResetPasswordDto
	{
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "OTP is required")]
        public string Otp { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string NewPassword { get; set; }
    }
}
