using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BusinessObject.Models
{
    [Table("Fees")]
    public class Fee
    {
        public int Id { get; set; } // Primary Key
        public int TotalFees { get; set; }
        public int PlatformFees { get; set; }
        public DateTime Date { get; set; }

    }
}
