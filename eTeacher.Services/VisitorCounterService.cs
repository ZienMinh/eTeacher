﻿using Repositories;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class VisitorCounterService : IVisitorCounterService
    {
        private readonly AddDbContext _context;

        public VisitorCounterService(AddDbContext context)
        {
            _context = context;
        }
        public int GetRegisteredUserCount()
        {
            return _context.Users.Count();
        }

        public int GetRequirementCount()
        {
            return _context.Requirements.Count();
        }

        public int GetReportCount()
        {
            return _context.Reports.Count();
        }
    }

}
