using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace eTeacher.Data
{
    [Table("Users")]
    public class User : IdentityUser
    {
		//[Required]
		//[MaxLength(50)]
		//public string User_name { get; set; }
		[Required]
        [MaxLength(50)]
        public string First_name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Last_name { get; set; }
        public byte Gender { get; set; }
		//[Required]
		//[MaxLength(50)]
		//public override string UserName { get; set; } 
        [MaxLength(50)]
        public string? Address { get; set; }
		[Required]
		[MaxLength(50)]
		public string Email { get; set; }
		[MaxLength(10)]
        public string? Wallet_id { get; set; } 
        public DateOnly Birth_date { get; set; }
        [MaxLength(100)]
        public string? Link_contact { get; set; }
        [Range(0, 5)]
        public byte Rating { get; set; }
        [MaxLength(100)]
        public string? Image { get; set; }
        public byte Role { get; set; }
		public ICollection<Requirement> Requirements { get; set; }
		public ICollection<Class> Classes { get; set; }
		public ICollection<Order> Orders { get; set; }
		public ICollection<Qualification> Qualifications { get; set; }
		public ICollection<Report> Reports { get; set; }    
	}
}
