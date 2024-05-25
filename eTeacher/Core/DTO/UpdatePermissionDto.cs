using System.ComponentModel.DataAnnotations;

namespace eTeacher.Core.DTO
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
    }
}
