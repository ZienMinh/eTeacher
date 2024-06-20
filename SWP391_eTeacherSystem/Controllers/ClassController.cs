using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWP391_eTeacherSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly AddDbContext _context;

        public ClassController(AddDbContext context)
        {
            _context = context;

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
