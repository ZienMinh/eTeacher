using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    [Table("Requirement")]
    public partial class Requirement
    {
        [Key]
        [MaxLength(10)]
        public string Requirement_id { get; set; }
        [MaxLength(450)]
        public string User_id { get; set; }
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
        [Required]
        public byte Grade { get; set; }
        [MaxLength(10)]
        public string Rank { get; set; }
        [Required]
        public double Price { get; set; }
        public int Number_of_session { get; set; }
    }
}
