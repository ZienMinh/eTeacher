using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace eTeacher.Data
{
    [Table("Order")]
    public class Order
	{
        [Key]
        [MaxLength(10)]
        public string Order_id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Wallet_id { get; set; }
        [Required]
        public DateTime Order_time { get; set; }
        [Required]
        [MaxLength(450)]
        public string User_id { get; set; }
        [MaxLength(10)]
        public string Class_id { get; set; }
        [Required]
        public byte Order_type { get; set; }
    }
}
