using API_Prototype_ASP.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_Prototype_ASP.Controllers


{
    public class ApiContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }
    }


}
