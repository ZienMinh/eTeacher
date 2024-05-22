using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eTeacher.Data
{
    [Table("Wallet")]
    public class Wallet
    {
        [Key]
        [MaxLength(10)]
        public string wallet_id { get; set; }
        [Required]

        public double balance { get; set; }
    }
}
