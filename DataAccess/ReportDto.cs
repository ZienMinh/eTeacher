using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ReportDto
    {
        public string Report_id { get; set; }
        public string? Student_id { get; set; }
        public string? Tutor_id { get; set; }
        public string? Class_id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content name is required")]
        public string Content { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public byte Rating { get; set; }
    }
}
