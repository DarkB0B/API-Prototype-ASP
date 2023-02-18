using API_Prototype_ASP.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_Prototype_ASP.Data


{
    public class ApiContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }
    }

    
}
