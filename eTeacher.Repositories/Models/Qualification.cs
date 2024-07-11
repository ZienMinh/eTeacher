using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    [Table("Qualification")]
    public partial class Qualification
    {
        [Key]
        [MaxLength(10)]
        public string Qualification_id { get; set; }
        [MaxLength(450)]
        public string User_id { get; set; }
        public int? Graduation_year { get; set; }
        [MaxLength(50)]
        public string? Specialize { get; set; }
        [MaxLength(10)]
        public string? Classification { get; set; }
        [MaxLength(50)]
        public string? Training_facility { get; set; }
        [MaxLength(100)]
        public string? Image { get; set; }
    }
}
