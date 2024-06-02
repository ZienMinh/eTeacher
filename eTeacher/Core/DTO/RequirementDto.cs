using System.ComponentModel.DataAnnotations;

namespace eTeacher.Core.DTO
{
    public class RequirementDto
    {
        public string Requirement_id { get; set; }
        public string User_id { get; set; }
        public string Subject_id { get; set; }
        public string Subject_name { get; set; }
        public DateOnly Start_date { get; set; }
        public DateOnly End_date { get; set; }
        public TimeOnly Start_time { get; set; }
        public TimeOnly End_time { get; set; }
        public byte Grade { get; set; }
        public string Rank { get; set; }
        public double Price { get; set; }
        [Required(ErrorMessage = "Number of session is required")]
        [MaxLength(10, ErrorMessage = "Maximum number of lessons 10 numeric characters")]
        public int Number_of_session { get; set; }
    }
}
