using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IReportService
    {
        Task<ReportServiceResponseDto> CreateFeedbackAsync(ReportDto reportDto);

        Task<ReportServiceResponseDto> GetFeedbackAsync(string classId);

        Task<ReportServiceResponseDto> GetReportAsync(string classId);

        Task<ReportServiceResponseDto> GetReportByIdAsync(string reportId);

        Task<ReportServiceResponseDto> GetAllReportAsync(ReportDto reportDto);

        Task<ReportServiceResponseDto> UpdateReportAsync(ReportDto reportDto);

        Task<ReportServiceResponseDto> SendEmailAsync(string userId, string subject, string message);

        string GenerateReportId();
    }
}
