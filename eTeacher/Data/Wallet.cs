using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace eTeacher.Data
{
    [Table("Wallet")]
    public class Wallet
	{
        [Key]
        [MaxLength(10)]
        public string Wallet_id { get; set; }
        [Required]
        public double Balance { get; set; }
    }
}
