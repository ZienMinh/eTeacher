using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eTeacher.Data
{
    [Table("Qualification")]
    public class Qualification
    {
        [Key]
        [MaxLength(10)]
        public string qualification_id { get; set; }

        [MaxLength(10)]
        public string user_id { get; set; }

        public int graduation_year { get; set; }
        [MaxLength(50)]
        public string specialize { get; set; }
        [MaxLength(10)]
        public string classification { get; set; }
        [MaxLength(50)]
        public string training_facility { get; set; }
        [MaxLength(100)]
        public string image { get; set; }

    }
}
