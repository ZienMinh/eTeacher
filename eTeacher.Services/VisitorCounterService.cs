using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories;

namespace Services
{
    public class VisitorCounterService
    {
        private int _totalVisitors;

        public int TotalVisitors
        {
            get { return _totalVisitors; }
        }

        
        public void IncrementVisitorCount()
        {
            _totalVisitors++;
        }
    }
}
