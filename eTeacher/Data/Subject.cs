using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTeacher.Data
{
    [Table("Subject")]
    public class Subject
    {
        [Required]
        [MaxLength(10)]
        public string Subject_id { get; set; }
        [Required]
        [MaxLength(450)]
        public string Subject_name { get; set; }
    }
}
