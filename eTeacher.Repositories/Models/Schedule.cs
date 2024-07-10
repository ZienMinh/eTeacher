using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        public string Class_id { get; set; }

        [ForeignKey("Class_id")]
        public Class Class { get; set; }

        [Required]
        public string? Student_id { get; set; }

        [ForeignKey("Student_id")]
        public User Student { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public bool ReminderSent { get; set; }

        public bool IsDeleted { get; set; }
    }
}
