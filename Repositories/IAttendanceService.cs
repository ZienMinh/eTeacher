using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAttendanceService
    {

        Task<AttendanceServiceResponseDto> GetAttendanceAsync(string classId);
        Task<AttendanceServiceResponseDto> CreateAttendanceAsync(AttendanceDto attendanceDto);
        string GenerateAttendanceId();
        Task<int> GetNextSlotNumber(string classId);
    }
}
