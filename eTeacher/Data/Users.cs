using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eTeacher.Data
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [MaxLength(10)]
        public string user_id { get; set; }
        [Required]
        [MaxLength(50)]
        public string user_name { get; set; }
        [Required]
        [MaxLength(50)]
        public string first_name { get; set; }
        [Required]
        [MaxLength(50)]
        public string last_name { get; set; }
        [Required]
        [MaxLength(10)]
        public string password { get; set; }
        public byte gender { get; set; }
        [Required]
        [MaxLength(50)]
        public string email { get; set; }
        [Required]
        [MaxLength(50)]
        public string address { get; set; }
        [Required]
        public int phone_number { get; set; }
        [MaxLength(10)]
        public string wallet_id { get; set; } 

        public DateOnly birth_date { get; set; }

        [MaxLength(100)]
        public string link_contact { get; set; }
        [Range(0, 5)]
        public byte rating { get; set; }
        [MaxLength(100)]
        public string image { get; set; }
        [Required]
        public byte role { get; set; }
    }
}
