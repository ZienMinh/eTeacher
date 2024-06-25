using eTeacher.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eTeacher.Core.DTO
{
    public class ClassDto
    {
        public string Class_id { get; set; }
        public string? Address { get; set; }
        public string Student_id { get; set; }
        public User Student { get; set; }
        public string Tutor_id { get; set; }
        public User Tutor { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        public string Subject_name { get; set; }
        public Subject Subject { get; set; }
        public DateOnly Start_date { get; set; }
        public DateOnly End_date { get; set; }
        public TimeOnly Start_time { get; set; }
        public TimeOnly End_time { get; set; }

        [Required(ErrorMessage = "Type_class is required")]
        public byte Type_class { get; set; }
        public double Price { get; set; }

        [Required(ErrorMessage = "Number of session is required")]
        [MaxLength(10, ErrorMessage = "Maximum number of lessons 10 numeric characters")]
        public int Number_of_session { get; set; }
    }
}
