using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    [Table("Report")]
    public partial class Report
    {
        [Key]
        [MaxLength(10)]
        public string Report_id { get; set; }
        [MaxLength(450)]
        public string? Student_id { get; set; }
        [ForeignKey("Student_id")]
        public User Student { get; set; }
        [MaxLength(450)]
        public string? Tutor_id { get; set; }
        [ForeignKey("Tutor_id")]
        public User Tutor { get; set; }
        [MaxLength(10)]
        public string? Class_id { get; set; }
        [ForeignKey("Class_id")]
        public Class Class { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(750)]
        public string Content { get; set; }

        public byte Rating { get; set; }
    }
}
