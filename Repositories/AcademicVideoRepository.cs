using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AcademicVideoRepository : IAcademicVideoRepository
    {
        private readonly AddDbContext _context;

        public AcademicVideoRepository(AddDbContext context)
        {
            _context = context;
        }

        public async Task<string> UploadVideoAsync(IFormFile videoFile)
        {
            var directoryPath = Path.Combine("wwwroot", "videos");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, Guid.NewGuid() + Path.GetExtension(videoFile.FileName));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
            }

            return filePath.Substring("wwwroot".Length); 
        }

        public async Task<AcademicVideo> AddVideoAsync(AcademicVideo video)
        {
            _context.AcademicVideos.Add(video);
            await _context.SaveChangesAsync();
            return video;
        }

        public async Task<IEnumerable<AcademicVideo>> GetAllVideosAsync()
        {
            return await _context.AcademicVideos.ToListAsync();
        }
    }
}
