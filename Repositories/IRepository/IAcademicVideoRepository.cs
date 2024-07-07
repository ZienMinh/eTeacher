using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
	public interface IAcademicVideoRepository
	{
		Task<string> UploadVideoAsync(IFormFile videoFile);
		Task<AcademicVideo> AddVideoAsync(AcademicVideo video);
		Task<IEnumerable<AcademicVideo>> GetAllVideosAsync();

    }
}
