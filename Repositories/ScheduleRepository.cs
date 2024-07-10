using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly AddDbContext _context;

        public ScheduleRepository(AddDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Schedule>> GetUpcomingSchedulesAsync()
        {
            return await _context.Schedules
                .Include(s => s.Student)
                .Include(s => s.Class)
                .Where(s => s.StartTime > DateTime.Now && s.StartTime <= DateTime.Now.AddMinutes(5) && !s.ReminderSent && !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByStudentIdAsync(string studentId)
        {
            return await _context.Schedules
                .Include(s => s.Class)
                .ThenInclude(c => c.Subject)
                .Where(s => s.Student_id == studentId && !s.IsDeleted)
                .ToListAsync();
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSchedulesByClassIdAsync(string classId)
        {
            var schedules = await _context.Schedules.Where(s => s.Class_id == classId).ToListAsync();
            foreach (var schedule in schedules)
            {
                schedule.IsDeleted = true;
            }
            _context.Schedules.UpdateRange(schedules);
            await _context.SaveChangesAsync();
        }
    }
}
