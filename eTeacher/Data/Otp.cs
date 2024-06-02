using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTeacher.Data
{
    [Table("Otp")]
    public class Otp
    {
        [Required]
        [MaxLength(10)]
        public string Otp_id { get; set; }
        [Required]
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
