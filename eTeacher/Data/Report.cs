using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace eTeacher.Data
{
    [Table("Report")]
    public class Report
	{
        [Key]
        [MaxLength(10)]
        public string Report_id { get; set; }
        [Required]
        [MaxLength(450)]
        public string User_id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Class_id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(750)]
        public string Content { get; set; }
    }
}
