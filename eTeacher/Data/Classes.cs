using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eTeacher.Data
{
    [Table("Classes")]
    public class Classes
    {
        [Key]
        [MaxLength(10)]
        public string class_id { get; set; }

        [MaxLength(50)]
        public string address { get; set; }
        [MaxLength(10)]
        public string student_id { get; set; }
        [MaxLength(10)]
        public string tutor_id { get; set; }

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

        public byte type_class { get; set; }

        public double price { get; set; }

        public int number_of_session { get; set; }
    }
}
