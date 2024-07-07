using BusinessObject.Models;
using DataAccess;
using eTeacher.Core.Services;
using eTeacher.Services;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public ReportService(AddDbContext context, ILogger<ReportService> logger, UserManager<User> userManager, IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<ReportServiceResponseDto> CreateFeedbackAsync(ReportDto reportDto)
        {
            _logger.LogInformation("Create feedback");
            try
            {
                var newReportId = GenerateReportId();

                var newReport = new Report
                {
                    Report_id = newReportId,
                    Student_id = reportDto.Student_id,
                    Tutor_id = reportDto.Tutor_id,
                    Class_id = reportDto.Class_id,
                    Title = reportDto.Title,
                    Content = reportDto.Content,
                    Rating = reportDto.Rating,
                    Processing = reportDto.Processing,
                    Type = reportDto.Type,
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

        public async Task<ReportServiceResponseDto> GetFeedbackAsync(string classId)
        {
            var feedbacks = await _context.Reports
                                          .Where(r => r.Class_id == classId && r.Type == 1)
                                          .ToListAsync();

            if (feedbacks.Any())
            {
                return new ReportServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Feedbacks found.",
                    Reports = feedbacks.Select(f => new ReportDto
                    {
                        Report_id = f.Report_id,
                        Student_id = f.Student_id,
                        Tutor_id = f.Tutor_id,
                        Class_id = f.Class_id,
                        Title = f.Title,
                        Content = f.Content,
                        Rating = f.Rating
                    }).ToList()
                };
            }
            else
            {
                return new ReportServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No feedbacks found for the given class ID.",
                    Reports = null
                };
            }
        }

        public async Task<ReportServiceResponseDto> GetReportAsync(string classId)
        {
            var feedbacks = await _context.Reports
                                          .Where(r => r.Class_id == classId && r.Type == 2)
                                          .ToListAsync();

            if (feedbacks.Any())
            {
                return new ReportServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Feedbacks found.",
                    Reports = feedbacks.Select(f => new ReportDto
                    {
                        Report_id = f.Report_id,
                        Student_id = f.Student_id,
                        Tutor_id = f.Tutor_id,
                        Class_id = f.Class_id,
                        Title = f.Title,
                        Content = f.Content,
                        Processing = f.Processing,
                        Rating = f.Rating
                    }).ToList()
                };
            }
            else
            {
                return new ReportServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No feedbacks found for the given class ID.",
                    Reports = new List<ReportDto>()
                };
            }
        }

        public async Task<ReportServiceResponseDto> GetReportByIdAsync(string reportId)
        {
            var feedbacks = await _context.Reports
                                          .Where(r => r.Report_id == reportId && r.Type == 2)
                                          .ToListAsync();

            if (feedbacks.Any())
            {
                return new ReportServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Feedbacks found.",
                    Reports = feedbacks.Select(f => new ReportDto
                    {
                        Report_id = f.Report_id,
                        Student_id = f.Student_id,
                        Tutor_id = f.Tutor_id,
                        Class_id = f.Class_id,
                        Title = f.Title,
                        Content = f.Content,
                        Processing = f.Processing,
                        Rating = f.Rating
                    }).ToList()
                };
            }
            else
            {
                return new ReportServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "No feedbacks found for the given class ID.",
                    Reports = new List<ReportDto>()
                };
            }
        }

        public async Task<ReportServiceResponseDto> GetAllReportAsync(ReportDto reportDto)
        {
            var reports = await _context.Reports
                    .Where(report => report.Type == 2)
                    .OrderBy(report => report.Processing)
                    .ToListAsync();

            var reportDtos = reports.Select(report => new ReportDto
            {
                Report_id = report.Report_id,
                Class_id = report.Class_id,
                Student_id = report.Student_id,
                Tutor_id = report.Tutor_id,
                Title = report.Title,
                Content = report.Content,
                Processing = report.Processing
            }).ToList();

            if (reportDtos.Any())
            {
                return new ReportServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Reports retrieved successfully.",
                    Reports = reportDtos
                };
            }
            else
            {
                return new ReportServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "An error occurred",
                    Reports = new List<ReportDto>()
                };
            }
        }

        public async Task<ReportServiceResponseDto> UpdateReportAsync(ReportDto reportDto)
        {
            var response = new ReportServiceResponseDto();

            try
            {
                var reports = await _context.Reports.FindAsync(reportDto.Report_id);

                if (reports == null)
                {
                    response.IsSucceed = false;
                    response.Message = "Report not found";
                    return response;
                }

                reports.Processing = reportDto.Processing != default(byte) ? reportDto.Processing : reports.Processing;

                await _context.SaveChangesAsync();

                response.IsSucceed = true;
                response.Message = "Report updated successfully";
                response.CreateReport = reports;
            }
            catch (Exception ex)
            {
                response.IsSucceed = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ReportServiceResponseDto> SendEmailAsync(string userId, string subject, string message)
        {
            _logger.LogInformation("SendEmailAsync started");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return new ReportServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "User not found"
                };
            }

            await _emailService.SendEmailAsync(user.Email, subject, message);
            _logger.LogInformation("Email sent to {Email}", user.Email);

            return new ReportServiceResponseDto()
            {
                IsSucceed = true,
                Message = "Email sent successfully"
            };
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
