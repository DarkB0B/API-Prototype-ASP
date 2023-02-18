using Microsoft.EntityFrameworkCore;


namespace API_Prototype_ASP.Models
{
    public class Reservation
    {
        
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public string? ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
