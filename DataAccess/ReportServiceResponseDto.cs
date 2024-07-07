using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ReportServiceResponseDto
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }

        public List<ReportDto> Reports { get; set; }

        public Report CreateReport {  get; set; }
    }
}
