using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AttendanceServiceResponseDto
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }

        public List<AttendanceDto> Data { get; set; }

    }
}
