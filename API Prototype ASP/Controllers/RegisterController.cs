using API_Prototype_ASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Prototype_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {

        private readonly ApiContext _context;
        public RegisterController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Register([FromBody] UserModel user)
        {

            if (_context.Users.Any(u => u.Username == user.Username))
            {
                return BadRequest("Username already exists");
            }
            user.Id = _context.Users.Count() + 1;
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();

        }
    }
}
