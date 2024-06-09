using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    [Table("Otp")]
    public partial class Otp
    {
        [Required]
        [MaxLength(10)]
        public string Otp_id { get; set; }
        [MaxLength(450)]
        public string User_id { get; set; }
        public User User { get; set; }
        [Required]
        [MaxLength(10)]
        public string Otp_code { get; set; }
        [Required]
        public DateTime Expiry_time { get; set; }
    }
}
