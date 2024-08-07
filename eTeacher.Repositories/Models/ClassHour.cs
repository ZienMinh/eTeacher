﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    [Table("ClassHour")]
    public partial class ClassHour
    {
        [Key]
        [MaxLength(10)]
        public string Class_id { get; set; }

        [MaxLength(50)]
        public string? Address { get; set; }

        [MaxLength(450)]
        public string User_id { get; set; }

        [MaxLength(450)]
        public string Subject_name { get; set; }

        public Subject Subject { get; set; }

        public DateOnly? Start_date { get; set; }

        public DateOnly? End_date { get; set; }

        public TimeOnly? Start_time { get; set; }

        public TimeOnly? End_time { get; set; }

        public byte Grade { get; set; }

        public byte Type_class { get; set; }

        public double Price { get; set; }

        public int Number_of_session { get; set; }

        [MaxLength(450)]
        public string? Description { get; set; }
    }
}
