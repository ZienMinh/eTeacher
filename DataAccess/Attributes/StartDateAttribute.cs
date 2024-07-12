using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Attributes
{
    public class StartDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateOnly startDate)
            {
                return startDate >= DateOnly.FromDateTime(DateTime.Now);
            }
            return false;
        }
    }
}
