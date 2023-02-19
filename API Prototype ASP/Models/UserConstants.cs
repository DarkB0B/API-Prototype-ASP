using API_Prototype_ASP.Controllers;

namespace API_Prototype_ASP.Models
{
    public class UserConstants
    {

        private readonly ApiContext _context;

        public UserConstants(ApiContext context)
        {
            _context = context;
        }

        public static List<UserModel> User = new List<UserModel>()
        {
            new UserModel { Username = "admin", Password = "admin", Name = "admin", Role = "Admin" },
            new UserModel { Username = "user", Password = "user", Name = "user", Role = "User" }

        };
    }
}
