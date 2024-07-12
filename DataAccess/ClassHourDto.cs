using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ClassHourDto
    {
        public string Class_id { get; set; }

        public string User_id { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        public string Subject_name { get; set; }

        public DateOnly? Start_date { get; set; }

        public DateOnly? End_date { get; set; }

        public TimeSpan? Start_time { get; set; }

        public TimeSpan? End_time { get; set; }

        [Required(ErrorMessage = "Grade is required")]
        [Range(1, 12, ErrorMessage = "Grade must be between 1 and 12")]
        public byte Grade { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Number of sessions is required")]
        [Range(1, 100, ErrorMessage = "Number of sessions must be between 1 and 100")]
        public int Number_of_session { get; set; }

        public string? Description { get; set; }

        public double? Total { get; set; }

        public byte? Status { get; set; }
    }
}
