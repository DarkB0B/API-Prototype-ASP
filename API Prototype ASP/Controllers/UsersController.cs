using API_Prototype_ASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Prototype_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApiContext _context;
        public UsersController(ApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(_context.Users.ToList());
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as System.Security.Claims.ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserModel
                {
                    Username = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                    Name = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                    Role = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                };
            }

            return null;
        }
        public class Editable
        {
            public string currentPassword { get; set; }
            public string? newName { get; set; }
            public string? newUsername { get; set; }
            public string? newPassword { get; set; }

        }
        [HttpPut]
        [Authorize]
        public ActionResult Edit([FromBody] Editable data)
        {
            var thisUser = GetCurrentUser();
            var currentUser = _context.Users.Where(u => u.Username == thisUser.Username).FirstOrDefault();
            Console.WriteLine(currentUser.Password);
            string result = "You changed your ";
            int counter = 0;
            if(currentUser == null)
            {
                return NotFound("User Not Found");
            }

            if(thisUser.Password != data.currentPassword)
            {
                if (data.newUsername != null)
                {
                    currentUser.Username = data.newUsername;
                    result += "username ";
                    counter++;
                }
                else if (data.newPassword != null)
                {
                    currentUser.Password = data.newPassword;
                    if (counter != 0)
                    {
                        result += "and ";
                    }
                    result += "password ";
                    counter++;
                }
                else if (data.newName != null)
                {
                    currentUser.Name = data.newName;
                    if (counter != 0)
                    {
                        result += "and ";
                    }
                    result += "Name";
                    counter++;
                }
                else
                {
                    return BadRequest("No changes were made");
                }
                
                _context.SaveChanges();

                return Ok(result);
                
            }
            else
            {
                return NotFound("Wrong Password");
            }
        }
        [HttpDelete]
        [Authorize]
        public ActionResult Delete([FromBody] UserLogin userCredentials)
        {
            var user = _context.Users.Where(u => u.Username == userCredentials.Username).FirstOrDefault();
            if(user == null)
            { return BadRequest("Couldnt Find This User"); };
            if(userCredentials.Password == user.Password)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return Ok("User was deleted");
            }

            return BadRequest("Wrong Password");
        }

    }

}
