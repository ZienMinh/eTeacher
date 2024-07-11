using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FeeDto
    {
        public int Id { get; set; }
        public List<string> Labels { get; set; }
        public List<int> TotalFeesData { get; set; }
        public List<int> PlatformFeesData { get; set; }
    }
}
