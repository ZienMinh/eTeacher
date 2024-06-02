using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace eTeacher.Data
{
    [Table("Classes")] 
	public class Class
	{
        [Key]
        [MaxLength(10)]
        public string Class_id { get; set; }
        [MaxLength(50)]
        public string? Address { get; set; }
		[Required]
		[MaxLength(450)] // Update to match Users.Id
        [Column("student_id")]
		public string Student_id { get; set; }
		[ForeignKey("student_id")]
		public User Student { get; set; }
		[Required]
		[MaxLength(450)]
        public string Tutor_id { get; set; }
        [Required]
        [MaxLength(450)]
        public string Subject_name { get; set; }
        public Subject Subject { get; set; }
        [Required]
        public DateOnly Start_date { get; set; }
        [Required]
        public DateOnly End_date { get; set; }
        [Required]
        public TimeOnly Start_time { get; set; }
        [Required]
        public TimeOnly End_time { get; set; }
        public byte Type_class { get; set; }
        public double Price { get; set; }
        public int Number_of_session { get; set; }
    }
}
