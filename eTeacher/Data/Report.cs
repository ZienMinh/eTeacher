using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eTeacher.Data
{
    [Table("Report")]
    public class Report
    {
        [Key]
        [MaxLength(10)]
        public string report_id { get; set; }

        [MaxLength(10)]
        public string user_id { get; set; }

        [Required]
        [MaxLength(50)]
        public string title { get; set; }
        [Required]
        [MaxLength(750)]
        public string content { get; set; }
    }
}
