using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWP391_eTeacherSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequirementController : ControllerBase
    {
        private readonly AddDbContext _context;

        public RequirementController(AddDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var dsRequirement = _context.Requirements.ToList();
            return Ok(dsRequirement);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var requirements = _context.Requirements.Where(r => r.User_id == id).ToList();
            if (requirements.Any())
            {
                return Ok(requirements);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult CreateRequirement(RequirementDto model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var userId = GetCurrentUserId();
                    if (userId == null)
                    {
                        return BadRequest("User is not authenticated.");
                    }

                    var subject = _context.Subjects.SingleOrDefault(s => s.Subject_name == model.Subject_name);
                    if (subject == null)
                    {
                        return BadRequest("Invalid Subject_name.");
                    }

                    var requirementId = GenerateRequirementId();

                    var requirement = new Requirement
                    {
                        Requirement_id = requirementId,
                        User_id = userId,
                        Subject_name = model.Subject_name,
                        Start_date = model.Start_date,
                        End_date = model.End_date,
                        Start_time = model.Start_time,
                        End_time = model.End_time,
                        Grade = model.Grade,
                        Rank = model.Rank,
                        Price = model.Price,
                        Number_of_session = model.Number_of_session
                    };

                    _context.Add(requirement);

                    _context.SaveChanges();
                    transaction.Commit();

                    return Ok(requirement);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
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
