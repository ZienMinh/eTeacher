using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BusinessObject.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string First_name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Last_name { get; set; }

        public byte Gender { get; set; }

        [MaxLength(50)]
        public string? Address { get; set; }

        [NotMapped]
        public DateOnly Birth_date { get; set; }

        [MaxLength(100)]
        public string? Link_contact { get; set; }

        [Range(0, 5)]
        public byte Rating { get; set; }

        [MaxLength(100)]
        public string? Image { get; set; }

        [Required]
        public byte Role { get; set; }

        public ICollection<Requirement> Requirements { get; set; }
        public ICollection<ClassHour> ClassHours { get; set; }
        public ICollection<Class> Classes { get; set; }
        public ICollection<Class> TutorClasses { get; set; }
        public ICollection<Class> StudentClasses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Qualification> Qualifications { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
