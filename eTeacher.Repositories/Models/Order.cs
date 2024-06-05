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
        [MaxLength(10)]
        public string Order_id { get; set; }
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
