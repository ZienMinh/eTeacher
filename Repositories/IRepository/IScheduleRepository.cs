using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetUpcomingSchedulesAsync();
        Task<IEnumerable<Schedule>> GetSchedulesByStudentIdAsync(string studentId);
        Task UpdateScheduleAsync(Schedule schedule);
        Task DeleteSchedulesByClassIdAsync(string classId);
    }
}
