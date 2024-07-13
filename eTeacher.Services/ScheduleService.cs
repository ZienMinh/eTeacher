using BusinessObject.Models;
using Repositories.IRepository;
using Repositories;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<ReportService> _logger;
        private readonly UserManager<User> _userManager;

        public ScheduleService(IScheduleRepository scheduleRepository, IEmailService emailService, UserManager<User> userManager, ILogger<ReportService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByStudentIdAsync(string studentId)
        {
            return await _scheduleRepository.GetSchedulesByStudentIdAsync(studentId);
        }

        public async Task SendClassRemindersAsync()
        {
            var schedules = await _scheduleRepository.GetUpcomingSchedulesAsync();

            foreach (var schedule in schedules)
            {
                var studentEmail = schedule.Student.Email;
                var subject = "Reminder: Upcoming Class";
                var body = $"Dear {schedule.Student.UserName},<br/><br/>" +
                           $"This is a reminder for your upcoming class:<br/>" +
                           $"Class: {schedule.Class.Subject_name}<br/>" +
                           $"Start Time: {schedule.StartTime}<br/><br/>" +
                           $"Please be prepared.<br/><br/>" +
                           $"Best regards,<br/>" +
                           $"eTeacher";

                await _emailService.SendEmailAsync(studentEmail, subject, body);

                schedule.ReminderSent = true;
                await _scheduleRepository.UpdateScheduleAsync(schedule);
            }
        }

        public async Task DeleteSchedulesByClassIdAsync(string classId)
        {
            await _scheduleRepository.DeleteSchedulesByClassIdAsync(classId);
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
    }
}
