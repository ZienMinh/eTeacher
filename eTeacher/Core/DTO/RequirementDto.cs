using System.ComponentModel.DataAnnotations;

namespace eTeacher.Core.DTO
{
    public class RequirementDto
    {
        public string Requirement_id { get; set; }
        public string User_id { get; set; }
        public string Subject_id { get; set; }
<<<<<<< HEAD
=======

        [Required(ErrorMessage = "Subject name is required")]
>>>>>>> test
        public string Subject_name { get; set; }
        public DateOnly Start_date { get; set; }
        public DateOnly End_date { get; set; }
        public TimeOnly Start_time { get; set; }
        public TimeOnly End_time { get; set; }
<<<<<<< HEAD
        public byte Grade { get; set; }
        public string Rank { get; set; }
=======

        [Required(ErrorMessage = "Grade is required")]
        [MaxLength(2, ErrorMessage = "Maximum Grade 2 numeric characters")]
        public byte Grade { get; set; }

        [Required(ErrorMessage = "Rank is required")]
        public string Rank { get; set; }

        [Required(ErrorMessage = "Price is required")]
>>>>>>> test
        public double Price { get; set; }
        [Required(ErrorMessage = "Number of session is required")]
        [MaxLength(10, ErrorMessage = "Maximum number of lessons 10 numeric characters")]
        public int Number_of_session { get; set; }
    }
}
