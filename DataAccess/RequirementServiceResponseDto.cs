﻿using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RequirementServiceResponseDto
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }

        public Requirement CreatedRequirement { get; set; }

        public List<Requirement> Requirements { get; set; }
    }
}
