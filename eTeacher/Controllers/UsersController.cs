//using eTeacher.Data;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace eTeacher.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private readonly AddDbContext _context;

//        public UsersController(AddDbContext context)
//        {
//            _context = context;

//        }

//        [HttpGet]
//        public IActionResult GetAllUser() {
//            var dsUsers = _context.Users.ToList();
//            return Ok(dsUsers);
//        }

//        [HttpGet("{id}")]
//        public IActionResult GetUserById(string id)
//        {
//            var users = _context.Users.SingleOrDefault(lo => lo.user_id == id);
//            if (users != null) {
//                return Ok(users);
//            }
//            else
//            {
//                return NotFound();
//            }
//        }

//        /*[HttpPost]
//        public IActionResult CreateNewUser(Users users)
//        {
//            try
//            {
//                //var user = new Users
//                {
//                    user_id = users.user_id,
//                    user_name = users.user_name,
//                    first_name = users.first_name,
//                    last_name = users.last_name,
//                    password = users.password,
//                    gender = users.gender,
//                    email = users.email,
//                    address = users.address,
//                    phone_number = users.phone_number,
//                    wallet_id = users.wallet_id,
//                    birth_date = users.birth_date,
//                    link_contact = users.link_contact,
//                    image = users.image,
//                    rating = users.rating,
//                    role = users.role,
//                };
//                _context.Add(users);
//                _context.SaveChanges();
//                return Ok(users);
//            } catch (Exception ex) { 
//                return BadRequest(ex.Message);
//            }
//        }*/


//        [HttpPost]
//        public IActionResult CreateUser(Users newUser)
//        {
//            if (ModelState.IsValid)
//            {
//                var users = new Users
//                {
//                    user_id = newUser.user_id,
//                    user_name = newUser.user_name,
//                    first_name = newUser.first_name,
//                    last_name = newUser.last_name,
//                    password = newUser.password,
//                    gender = newUser.gender,
//                    email = newUser.email,
//                    address = newUser.address,
//                    phone_number = newUser.phone_number,
//                    wallet_id = newUser.wallet_id,
//                    birth_date = newUser.birth_date,
//                    link_contact = newUser.link_contact,
//                    image = newUser.image,
//                    rating = newUser.rating,
//                    role = newUser.role,
//                };
//                _context.Users.Add(newUser);
//                _context.SaveChanges();
//                return CreatedAtAction(nameof(GetAllUser), new { id = newUser.user_id }, newUser);
//            }
//            return BadRequest(ModelState);
//        }

//    }

//}
