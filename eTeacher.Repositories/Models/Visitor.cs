using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BusinessObject.Models
{
    [Table("Visitor")]
    public class Visitor
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }

    }
}
