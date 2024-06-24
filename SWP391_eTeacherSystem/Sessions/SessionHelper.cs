using DataAccess;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_eTeacherSystem.Helpers
{
    public class SessionHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddClassToSession(ClassDto classDto)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var classList = session.Get<List<ClassDto>>("ClassList") ?? new List<ClassDto>();
            classList.Add(classDto);
            session.Set("ClassList", classList);
        }

        public List<ClassDto> GetClassListFromSession()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            return session.Get<List<ClassDto>>("ClassList") ?? new List<ClassDto>();
        }
    }
}
