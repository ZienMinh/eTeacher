using BusinessObject.Models;
using DataAccess;
using eTeacher.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReportService : IReportService
    {
        private readonly AddDbContext _context;
        private readonly ILogger<ReportService> _logger;

        public ReportService(AddDbContext context, ILogger<ReportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ReportServiceResponseDto> CreateFeedbackAsync(ReportDto reportDto)
        {
            _logger.LogInformation("Create feedback");
            try
            {
                var newReportId = GenerateReportId();

                // Create new report
                var newReport = new Report
                {
                    Report_id = newReportId,
                    Student_id = reportDto.Student_id,
                    Tutor_id = reportDto.Tutor_id,
                    Class_id = reportDto.Class_id,
                    Title = reportDto.Title,
                    Content = reportDto.Content,
                    Rating = reportDto.Rating,
                };

                _context.Reports.Add(newReport);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Feedback created successfully with Report ID: {ReportID}", newReportId);

                return new ReportServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Feedback created successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating feedback: {Error}", ex.Message);
                return new ReportServiceResponseDto
                {
                    IsSucceed = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public string GenerateReportId()
        {
            var maxReportId = _context.Reports
                .OrderByDescending(c => c.Report_id)
                .Select(c => c.Report_id)
                .FirstOrDefault();

            int currentCount = 0;

            if (maxReportId != null && maxReportId.StartsWith("R") && int.TryParse(maxReportId.Substring(1), out int parsedId))
            {
                currentCount = parsedId;
            }

            return "R" + (currentCount + 1).ToString("D9");
        }
    }
}
