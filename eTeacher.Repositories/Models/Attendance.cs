using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        [MaxLength(10)]
        public string Attendance_id { get; set; }

        [MaxLength(10)]
        public string? Class_id { get; set; }
        [ForeignKey("Class_id")]
        public Class Class { get; set; }
        public DateTime? Date { get; set; }

        public int? Slot { get; set; }

        public byte? Status { get; set; }

    }
}
