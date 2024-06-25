using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace SWP391_eTeacherSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AddDbContext _context;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(AddDbContext context, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserDto userDto)
        {
            var dsUser = await _context.Users.ToListAsync();

            var response = new UserServiceResponseDto
            {
                Users = dsUser
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var userDto = new UserDto();
            var response = await _userService.GetByIdAsync(id);

            if (response.IsSucceed)
            {
                return Ok(response.Users);
            }
            else
            {
                return NotFound(response.Message);
            }
        }


        [HttpPost]
        public IActionResult CreateUser(UserDto model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var users = new User
                    {
                        UserName = model.UserName,
                        First_name = model.First_name,
                        Last_name = model.Last_name,
                        PasswordHash = model.PasswordHash,
                        Gender = model.Gender,
                        Email = model.Email,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        Birth_date = model.Birth_date,
                        Link_contact = model.Link_contact,
                        Image = model.Image,
                        Rating = model.Rating,
                        Role = model.Role,
                    };


                    _context.Add(users);

                    _context.SaveChanges();
                    transaction.Commit();

                    return Ok(users);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }



        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, UserDto model)
        {
            var users = _context.Users.SingleOrDefault(lo => lo.Id == id);
            if (users != null)
            {
                users.UserName = model.UserName;
                users.First_name = model.First_name;
                users.Last_name = model.Last_name;
                users.PasswordHash = model.PasswordHash;
                users.Gender = model.Gender;
                users.Email = model.Email;
                users.Address = model.Address;
                users.PhoneNumber = model.PhoneNumber;
                users.Birth_date = model.Birth_date;
                users.Link_contact = model.Link_contact;
                users.Image = model.Image;
                users.Rating = model.Rating;
                users.Role = model.Role;
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteUser(string id)
        {
            var users = _context.Users.SingleOrDefault(lo => lo.Id == id);
            if (users != null)
            {
                _context.Remove(users);
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
