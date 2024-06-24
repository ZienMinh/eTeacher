using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SWP391_eTeacherSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassHourController : ControllerBase
    {
        private readonly AddDbContext _context;
        private readonly IClassHourService _classHourService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClassHourController(AddDbContext context, IClassHourService classHourService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _classHourService = classHourService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ClassHourDto classHourDto)
        {
            var dsClassHour = await _context.ClassHours.ToListAsync();

            var response = new ClassHourServiceResponseDto
            {
                Classes = dsClassHour
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var classHourDto = new ClassHourDto();
            var response = await _classHourService.GetByIdAsync(classHourDto, id);

            if (response.IsSucceed)
            {
                return Ok(response.Classes);
            }
            else
            {
                return NotFound(response.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass([FromForm] ClassHourDto model)
        {
            var userId = _classHourService.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var response = await _classHourService.CreateClassAsync(model, userId);
            if (response.IsSucceed)
            {
                return Ok("OK");
            }

            return BadRequest(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequirement(string id)
        {
            var result = await _classHourService.DeleteClassAsync(id);
            if (result.IsSucceed)
            {
                return NoContent();
            }
            else
            {
                return NotFound(result.Message);
            }
        }


        public string GetCurrentUserId()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        private string GenerateRequirementId()
        {
            int currentCount = _context.Requirements.Count();
            return "R" + (currentCount + 1).ToString("D9");
        }
    }
}
