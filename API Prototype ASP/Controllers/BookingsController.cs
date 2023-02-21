using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Prototype_ASP.Models;
using API_Prototype_ASP.Controllers;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace API_Prototype_ASP.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
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

        private readonly ApiContext _context;
        public BookingsController(ApiContext context)
        {
            _context = context;
        }
        
        // Create
        [HttpPost("book/")]
        [Authorize]
        public JsonResult Create(int roomno, DateTime startDate, DateTime endDate)
        {
            var currentuser = GetCurrentUser();
            Reservation reservation = new Reservation();
            reservation.ClientName = currentuser.Name;
            reservation.RoomNumber = roomno;
            reservation.StartDate = startDate;
            reservation.EndDate = endDate;

            if (reservation == null || reservation.RoomNumber == 0 || reservation.RoomNumber > 5 || reservation.ClientName == null || reservation.StartDate == null || reservation.EndDate == null || reservation.StartDate > reservation.EndDate || reservation.StartDate < DateTime.Now)
            {
                return new JsonResult("Invalid reservation");
            }
            var reservationsForThisRoom = _context.Reservations.Where(r => (r.RoomNumber == reservation.RoomNumber) && (reservation.StartDate >= DateTime.Now)).ToList();
            foreach (var res in reservationsForThisRoom)
            {
                if (reservation.StartDate >= res.StartDate && reservation.StartDate <= res.EndDate)
                {
                    return new JsonResult("Room is already booked for this date");
                }
                if (reservation.EndDate >= res.StartDate && reservation.EndDate <= res.EndDate)
                {
                    return new JsonResult("Room is already booked for this date");
                }
            }

            reservation.Id = _context.Reservations.Count() + 1;
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                return new JsonResult(Ok(reservation));

        }
       
        //Get rooms reserved for specific user 
        [HttpGet()]
        [Authorize]
        public JsonResult GetUserReservations()
        {
            var currentuser = GetCurrentUser();
            if(currentuser.Role == "Administrator")
            {
                return new JsonResult(_context.Reservations.ToList());
            }
            var reservations = _context.Reservations.Where(r => r.ClientName == currentuser.Name).ToList();
            if (reservations.Count == 0)
            {
                return new JsonResult("No reservations found for this user");
            }
            return new JsonResult(reservations);
        }
       
        //Get list of reservations for room
        [HttpGet("for")]
        [Authorize]
        public JsonResult GetRoomReservations(int roomno)
        {
            var reservations = _context.Reservations.Where(r => r.RoomNumber == roomno && r.StartDate > DateTime.Now).ToList();
            if (reservations.Count == 0)
            {
                return new JsonResult("No reservations found for this room");
            }
            return new JsonResult(reservations);
        }
        //Get list of reservations for time period 
        [HttpGet("period")]
        [Authorize]
        public JsonResult GetPeriodReservations(DateTime startDate, DateTime endDate)
        {
            var reservations = _context.Reservations.Where(r => r.StartDate >= startDate && r.EndDate <= endDate).ToList();
            if (reservations.Count == 0)
            {
                return new JsonResult("No reservations found for this period");
            }
            return new JsonResult(reservations);
        }

    }
}
