using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    [Table("Order")]
    public partial class Order
    {
        [Key]
        [MaxLength(50)]
        public string Order_id { get; set; }

        [Required]
        public DateTime Order_time { get; set; }

        [MaxLength(450)]
        public string User_id { get; set; }

        [MaxLength(10)]
        public string Class_id { get; set; }

        [Required]
        public byte Order_type { get; set; }

        [Required]
        public byte Payment_status { get; set; }

        [MaxLength(20)]
        public string Transaction_id { get; set; }

        [Required]
        public double Amount { get; set; }

		[Required]
		public int CompletedSessions { get; set; }

		[Required]
		public double Price_per_session { get; set; }

		[Required]
		public double RefundAmount { get; set; }

		[ForeignKey("User_id")]
        public User User { get; set; }

        [ForeignKey("Class_id")]
        public Class Class { get; set; }
    }
}
