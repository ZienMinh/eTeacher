using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models
{
    [Table("Classes")]
    public partial class Class
    {
        [Key]
        [MaxLength(10)]
        public string Class_id { get; set; }

        [MaxLength(50)]
        public string? Address { get; set; }

        [MaxLength(450)]
        public string Student_id { get; set; }

        [ForeignKey("Student_id")]
        public User Student { get; set; }

        [MaxLength(450)]
        public string Tutor_id { get; set; }

        [ForeignKey("Tutor_id")]
        public User Tutor { get; set; }

        [MaxLength(450)]
        public string Subject_name { get; set; }

        public Subject Subject { get; set; }

        [Required]
        [NotMapped]
        public DateOnly Start_date { get; set; }

        [Required]
        [NotMapped]
        public DateOnly End_date { get; set; }

        [Required]
        [NotMapped]
        public TimeOnly Start_time { get; set; }

        [Required]
        [NotMapped]
        public TimeOnly End_time { get; set; }

        public byte Grade { get; set; }

        public byte Type_class { get; set; }

        public double Price { get; set; }

        public int Number_of_session { get; set; }
    }
    
}
