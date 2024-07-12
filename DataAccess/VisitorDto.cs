using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class VisitorDto
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
