using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AttendanceDto
    {
        public string Attendance_id { get; set; }

        public string? Class_id { get; set; }

        public DateTime? Date { get; set; }

        public int? Slot { get; set; }

        public byte? Status { get; set; }
    }
}
