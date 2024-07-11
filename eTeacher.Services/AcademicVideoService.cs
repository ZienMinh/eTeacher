using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Repositories;
using Repositories.IRepository;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AcademicVideoService : IAcademicVideoService
    {
        private readonly IAcademicVideoRepository _videoRepository;

        public AcademicVideoService(IAcademicVideoRepository videoRepository)
        {
			_videoRepository = videoRepository;
        }

        public Task<string> UploadVideoAsync(IFormFile videoFile)
        {
            return _videoRepository.UploadVideoAsync(videoFile);
        }

        public Task<AcademicVideo> AddVideoAsync(AcademicVideo video)
        {
            return _videoRepository.AddVideoAsync(video);
        }

        public async Task<IEnumerable<AcademicVideo>> GetAllVideosAsync() 
        {
            return await _videoRepository.GetAllVideosAsync();
        }
    }
}
