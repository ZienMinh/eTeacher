using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
	[Table("AcademicVideos")]
	public class AcademicVideo
	{
		[Key]
		[MaxLength(50)]
		public Guid? VideoId { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(450)]
		public string? Tutor_id { get; set; }

		[Required]
		[MaxLength(100)]
		public string? Title { get; set; }
		
		[MaxLength(500)]
		public string? Description { get; set; }

		[Required]
		public string VideoUrl { get; set; }

		[ForeignKey("Tutor_id")]
		public User Tutor { get; set; }
	}
}
