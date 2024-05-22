using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eTeacher.Data
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [MaxLength(10)]
        public string order_id { get; set; }

        [Required]
        [MaxLength(10)]
        public string wallet_id { get; set; }

        [Required]
        public DateTime order_time { get; set; }
        [Required]
        [MaxLength(10)]
        public string user_id { get; set; }

        [MaxLength(10)]
        public string class_id { get; set; }

        [Required]

        public byte order_type { get; set; }
    }
}
