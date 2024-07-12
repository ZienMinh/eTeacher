using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class QualificationServiceResponseDto
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }

        public Qualification CreatedQualification { get; set; }

        public List<Qualification> Qualifications { get; set; }

    }
}
