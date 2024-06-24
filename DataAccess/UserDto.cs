using BusinessObject.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDto : IdentityUser
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
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        public DateOnly Birth_date { get; set; }
        [MaxLength(100)]
        public string? Link_contact { get; set; }
        [Range(0, 5)]
        public byte Rating { get; set; }
        [MaxLength(100)]
        public string? Image { get; set; }
        public byte Role { get; set; }

        public static implicit operator UserDto(UserServiceResponseDto v)
        {
            throw new NotImplementedException();
        }
    }
}
