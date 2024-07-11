﻿using BusinessObject.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly AddDbContext _context;
        private readonly ILogger<AttendanceService> _logger;

        public AttendanceService(AddDbContext context, ILogger<AttendanceService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AttendanceServiceResponseDto> GetAttendanceAsync(string classId)
        {
            _logger.LogInformation("Get attendance for class ID: {ClassID}", classId);
            try
            {
                var attendances = await _context.Attendances
                    .Where(a => a.Class_id == classId)
                    .ToListAsync();

                if (attendances == null || !attendances.Any())
                {
                    _logger.LogWarning("No attendance found for class ID: {ClassID}", classId);
                    return new AttendanceServiceResponseDto
                    {
                        IsSucceed = false,
                        Message = "No attendance records found for the specified class ID."
                    };
                }

                _logger.LogInformation("Attendance records retrieved successfully for class ID: {ClassID}", classId);
                return new AttendanceServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Attendance records retrieved successfully.",
                    Data = attendances.Select(a => new AttendanceDto
                    {
                        Attendance_id = a.Attendance_id,
                        Class_id = a.Class_id,
                        Date = a.Date,
                        Slot = a.Slot,
                        Status = a.Status
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving attendance: {Error}", ex.Message);
                return new AttendanceServiceResponseDto
                {
                    IsSucceed = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }


        public async Task<AttendanceServiceResponseDto> CreateAttendanceAsync(AttendanceDto attendanceDto)
        {
            _logger.LogInformation("Create attendance");
            try
            {
                var newAttendanceId = GenerateAttendanceId();
                var currentDate = DateTime.Now;

                var newAttendance = new Attendance
                {
                    Attendance_id = newAttendanceId,
                    Class_id = attendanceDto.Class_id,
                    Date = DateTime.Now,
                    Slot = await GetNextSlotNumber(attendanceDto.Class_id),
                    Status = attendanceDto.Status ?? 1 
                };

                _context.Attendances.Add(newAttendance);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Attendance created successfully with Attendance ID: {AttendanceID}", newAttendanceId);

                return new AttendanceServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Attendance created successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating attendance: {Error}", ex.Message);
                return new AttendanceServiceResponseDto
                {
                    IsSucceed = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public string GenerateAttendanceId()
        {
            var maxAttendanceId = _context.Attendances
                .OrderByDescending(c => c.Attendance_id)
                .Select(c => c.Attendance_id)
                .FirstOrDefault();

            int currentCount = 0;

            if (maxAttendanceId != null && maxAttendanceId.StartsWith("A") && int.TryParse(maxAttendanceId.Substring(1), out int parsedId))
            {
                currentCount = parsedId;
            }

            return "A" + (currentCount + 1).ToString("D9");
        }

        public async Task<int> GetNextSlotNumber(string classId)
        {
            var maxSlot = await _context.Attendances
                .Where(a => a.Class_id == classId)
                .MaxAsync(a => (int?)a.Slot);

            return maxSlot.HasValue ? maxSlot.Value + 1 : 1;
        }
    }
}