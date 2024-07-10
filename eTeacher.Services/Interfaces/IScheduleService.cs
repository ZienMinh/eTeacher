using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule>> GetSchedulesByStudentIdAsync(string studentId);
        Task SendClassRemindersAsync();
        Task DeleteSchedulesByClassIdAsync(string classId);
    }
}
