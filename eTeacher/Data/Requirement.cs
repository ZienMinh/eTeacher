using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace eTeacher.Data
{
    [Table("Requirement")]
    public class Requirement
    {
        [Key]
        [MaxLength(10)]
        public string Requirement_id { get; set; }
        [Required]
        [MaxLength(450)]
        public string User_id { get; set; }
        [MaxLength(10)]
        public string Subject_id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Subject_name { get; set; }
        [Required]
        public DateOnly Start_date { get; set; }
        [Required]
        public DateOnly End_date { get; set; }
        [Required]
        public TimeOnly Start_time { get; set; }
        [Required]
        public TimeOnly End_time { get; set; }
        [Required]
        public byte Grade { get; set; }
        [MaxLength(10)]
        public string Rank { get; set; }
        [Required]
        public double Price { get; set; }
        public int Number_of_session { get; set; }
    }
}
