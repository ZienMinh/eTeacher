using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eTeacher.Data
{
    [Table("Requirement")]
    public class Requirement
    {
        [Key]
        [MaxLength(10)]
        public string requirement_id { get; set; }
        [Required]
        [MaxLength(10)]
        public string user_id { get; set; }

        [MaxLength(10)]
        public string subject_id { get; set; }
        [Required]
        [MaxLength(50)]
        public string subject_name { get; set; }
        [Required]
        public DateOnly start_date { get; set; }
        [Required]
        public DateOnly end_date { get; set; }
        [Required]
        public TimeOnly start_time { get; set; }
        [Required]
        public TimeOnly end_time { get; set; }
        [Required]
        public byte grade { get; set; }
        [MaxLength(10)]
        public string rank { get; set; }
        [Required]
        public double price { get; set; }

        public int number_of_session { get; set; }
    }
}
