using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Prototype_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ApiContext _context;
        public RoomsController(ApiContext context)
        {
            _context = context;
        }

        //Get all free rooms on given date
        [AllowAnonymous]
        [HttpGet]
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
