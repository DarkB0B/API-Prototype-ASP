using API_Prototype_ASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Prototype_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiContext _context;
        public UserController(ApiContext context)
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
                    Username = userClaims.FirstOrDefault(c => c.Type == "Username").Value,
                    Name = userClaims.FirstOrDefault(c => c.Type == "Name").Value,
                    Role = userClaims.FirstOrDefault(c => c.Type == "Role").Value
                };
            }

            return null;
        }
    }
}
