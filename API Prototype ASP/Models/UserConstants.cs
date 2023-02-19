namespace API_Prototype_ASP.Models
{
    public class UserConstants
    {
        public static List<UserModel> User = new List<UserModel>()
        {
            new UserModel { Username = "admin", Password = "admin", Name = "admin",Role = "Admin" },
            new UserModel { Username = "user", Password = "user",Name = "user", Role = "User" }
        };
    }
}
