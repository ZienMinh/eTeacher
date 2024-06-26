using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SWP391_eTeacherSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequirementController : ControllerBase
    {
        private readonly AddDbContext _context;
        private readonly IRequirementService _requirementService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequirementController(AddDbContext context, IRequirementService requirementService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _requirementService = requirementService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] RequirementDto requirementDto)
        {
            var dsRequirement = await _context.Requirements.ToListAsync();

            var response = new RequirementServiceResponseDto
            {
                Requirements = dsRequirement
            };

            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var requirementDto = new RequirementDto();
            var response = await _requirementService.GetByIdAsync(requirementDto, id);

            if (response.IsSucceed)
            {
                return Ok(response.Requirements);
            }
            else
            {
                return NotFound(response.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateClass([FromForm] RequirementDto model)
        {
            var userId = _requirementService.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var response = await _requirementService.CreateRequirementAsync(model, userId);
            if (response.IsSucceed)
            {
                return Ok("OK");
            }

            return BadRequest(response.Message);
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


        [HttpDelete("{id}")]

        public IActionResult DeleteRequiment(string id)
        {
            var requirements = _context.Requirements.SingleOrDefault(lo => lo.Requirement_id == id);
            if (requirements != null)
            {
                _context.Remove(requirements);
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
