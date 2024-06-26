using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    [Table("Subject")]
    public partial class Subject
    {
        [Required]
        [MaxLength(10)]
        public string Subject_id { get; set; }
        [Required]
        [MaxLength(450)]
        public string Subject_name { get; set; }

        public ICollection<Requirement> Requirements { get; set; }
        public ICollection<Class> Classes { get; set; }
    }
}
