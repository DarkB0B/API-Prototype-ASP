using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Prototype_ASP.Models;
using API_Prototype_ASP.Data;

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
        [HttpPost]
        public JsonResult Create(Reservation reservation)
        {
            var reservationCheck = reservation.Id != 1;
            if (reservationCheck)
            {
                reservation.Id = _context.Reservations.Count() + 1;
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                return new JsonResult(Ok(reservation));
            }
            

        }


       
    }
}
