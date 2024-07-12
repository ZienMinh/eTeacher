using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAcademicVideoService
    {
		Task<string> UploadVideoAsync(IFormFile videoFile);
		Task<AcademicVideo> AddVideoAsync(AcademicVideo video);
        Task<IEnumerable<AcademicVideo>> GetAllVideosAsync();
    }
}
