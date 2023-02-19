using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Prototype_ASP.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace API_Prototype_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApiContext _context;
        public BookingController(ApiContext context)
        {
            _context = context;
        }
        // Create
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Create(Reservation reservation)
        {
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
        //Get all reservations
        [HttpGet]
        [Authorize]
        public JsonResult GetAll()
        {
            return new JsonResult(_context.Reservations.ToList());
        }
        //Get all free rooms on given date
        [AllowAnonymous]
        [HttpGet("free/{date}")]
        public JsonResult GetFreeRooms(DateTime date)
        {
            var reservations = _context.Reservations.Where(r => r.StartDate <= date && r.EndDate >= date).ToList();
            var freeRooms = new List<int>();
            for (int i = 1; i <= 4; i++)
            {
                if (!reservations.Any(r => r.RoomNumber == i))
                {
                    freeRooms.Add(i);
                }
            }
            return new JsonResult(freeRooms);
        }


    }
}
