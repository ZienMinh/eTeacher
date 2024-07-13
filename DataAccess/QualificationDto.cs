using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class QualificationDto
    {
        public string Qualification_id { get; set; }
        public string? User_id { get; set; }
        public int? Graduation_year { get; set; }
        public string? Specialize { get; set; }
        public string? Classification { get; set; }
        public string? Training_facility { get; set; }
        public string? Image { get; set; }
    }
}
