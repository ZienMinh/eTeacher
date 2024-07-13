using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	public class AcademicVideoDto
	{
		[Required]
		[MaxLength(450)]
		public string Tutor_id { get; set; }

		[Required]
		[MaxLength(255)]
		public string Title { get; set; }

		[Required]
		[MaxLength(255)]
		public string Description { get; set; }

		[Required]
		[MaxLength(255)]
		public string VideoUrl { get; set; }
	}
}
