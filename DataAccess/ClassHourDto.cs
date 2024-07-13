using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Attributes;

namespace DataAccess
{
    public class ClassHourDto
    {
        public string Class_id { get; set; }

        public string User_id { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        public string Subject_name { get; set; }

        [StartDate(ErrorMessage = "Start date must be today or later")]
        public DateOnly? Start_date { get; set; }

        [EndDate("Start_date", ErrorMessage = "End date must be after start date")]
        public DateOnly? End_date { get; set; }

        public TimeSpan? Start_time { get; set; }

        public TimeSpan? End_time { get; set; }

        [Required(ErrorMessage = "Grade is required")]
        [Range(1, 12, ErrorMessage = "Grade must be between 1 and 12")]
        public byte Grade { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Number of sessions is required")]
        [Range(1, 100, ErrorMessage = "Number of sessions must be between 1 and 100")]
        public int Number_of_session { get; set; }

        public string? Description { get; set; }

        public double? Total { get; set; }

        [Required(ErrorMessage = "Link meet is required")]
        public string Link_meet { get; set; }

        public byte? Status { get; set; }
    }
}
