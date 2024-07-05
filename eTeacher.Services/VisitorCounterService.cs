using Repositories;
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

        public async Task IncrementVisitorCountAsync()
        {
            var visitor = new Visitor { VisitDate = DateTime.UtcNow };
            _context.Visitors.Add(visitor);
            await _context.SaveChangesAsync();
        }

        public int GetVisitorCount()
        {
            return _context.Visitors.Count();
        }

        public int GetRegisteredUserCount()
        {
            return _context.Users.Count();
        }
    }

}
