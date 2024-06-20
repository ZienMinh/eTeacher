using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ClassDto
    {
        public string Class_id { get; set; }
        public string? Address { get; set; }
        public string? Student_id { get; set; }
        public string Tutor_id { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        public string Subject_name { get; set; }
        public DateOnly Start_date { get; set; }
        public DateOnly End_date { get; set; }
        public TimeOnly Start_time { get; set; }
        public TimeOnly End_time { get; set; }
        [Required(ErrorMessage = "Grade is required")]
        [Range(1, 12, ErrorMessage = "Grade must be between 1 and 12")]
        public byte Grade { get; set; }

        [Required(ErrorMessage = "Type_class is required")]
        public byte Type_class { get; set; }
        public double Price { get; set; }

        [Required(ErrorMessage = "Number of sessions is required")]
        [Range(1, 100, ErrorMessage = "Number of sessions must be between 1 and 100")]
        public int Number_of_session { get; set; }
    }
}
