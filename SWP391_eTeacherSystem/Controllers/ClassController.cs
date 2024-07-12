using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System.Security.Claims;

namespace SWP391_eTeacherSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly AddDbContext _context;
        private readonly IClassService _classService;

        public ClassController(AddDbContext context, IClassService classService)
        {
            _context = context;
            _classService = classService;
        }

        [HttpGet]
        public IActionResult GetAllClass()
        {
            var dsClasses = _context.Classes.ToList();
            return Ok(dsClasses);
        }

        [HttpGet("{id}")]
        public IActionResult GetClassById(string id)
        {
            var classes = _context.Classes.SingleOrDefault(lo => lo.Class_id == id);
            if (classes != null)
            {
                return Ok(classes);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{Student_id}")]
        public IActionResult GetClassByStudentId(string id)
        {
            var classes = _context.Classes.Where(lo => lo.Student_id == id).ToList();
            if (classes.Any())
            {
                return Ok(classes);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{Tutor_id}")]
        public IActionResult GetClassByTutorId(string id)
        {
            var classes = _context.Classes.Where(lo => lo.Tutor_id == id).ToList();
            if (classes.Any())
            {
                return Ok(classes);
            }
            else
            {
                return NotFound();
            }
        }
        /*
        [HttpPost("Getbytype")]
        public async Task<ActionResult<ClassServiceResponseDto>> GetByTypeAsync(ClassDto classDto, byte type)
        {
            var response = await _classService.GetByTypeAsync(classDto);
            return Ok(response);
        }*/

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateClass(ClassDto model)
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

                    var classId = GenerateClassId();

                    var classes = new Class
                    {
                        Class_id = classId,
                        Address = model.Address,
                        Student_id = null,
                        Tutor_id = userId,
                        Subject_name = model.Subject_name,
                        Start_date = model.Start_date,
                        End_date = model.End_date,
                        Start_time = model.Start_time,
                        End_time = model.End_time,
                        Type_class = 2,
                        Price = model.Price,
                        Number_of_session = model.Number_of_session
                    };

                    _context.Add(classes);

                    _context.SaveChanges();
                    transaction.Commit();

                    return Ok(classes);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        /*[HttpPost("updateStudent")]
        public async Task<ActionResult<ClassServiceResponseDto>> UpdateStudentAsync([FromBody] ClassDto classDto, [FromQuery] string userId)
        {
            if (classDto == null || string.IsNullOrEmpty(classDto.Class_id) || string.IsNullOrEmpty(classDto.Student_id) || string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ClassServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "Invalid request."
                });
            }

            var response = await _classService.UpdateStudentAsync(classDto, userId);
            return Ok(response);
        }*/

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private string GenerateClassId()
        {
            int currentCount = _context.Requirements.Count();
            return "C" + (currentCount + 1).ToString("D9");
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteClass(string id)
        {
            var classes = _context.Classes.SingleOrDefault(lo => lo.Class_id == id);
            if (classes != null)
            {
                _context.Remove(classes);
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
