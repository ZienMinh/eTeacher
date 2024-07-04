using DataAccess;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SWP391_eTeacherSystem.Helpers
{
    public class SessionHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string ClassSessionKey = "ClassList";

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddClassToSession(ClassDto classDto)
        {
            var classList = GetClassListFromSession();
            classList.Add(classDto);
            _httpContextAccessor.HttpContext.Session.SetString(ClassSessionKey, JsonSerializer.Serialize(classList));
        }

        public List<ClassDto> GetClassListFromSession()
        {
            var sessionData = _httpContextAccessor.HttpContext.Session.GetString(ClassSessionKey);
            return sessionData == null ? new List<ClassDto>() : JsonSerializer.Deserialize<List<ClassDto>>(sessionData);
        }
    }
}
