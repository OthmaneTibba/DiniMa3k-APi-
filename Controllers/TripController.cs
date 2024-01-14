using DiniM3ak.Data;
using DiniM3ak.Dtos.Trip;
using DiniM3ak.Entity;
using DiniM3ak.Enums;
using DiniM3ak.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiniM3ak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TripController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthUtils _authUtils;

        public TripController(AppDbContext context, AuthUtils authUtils)
        {
            _context = context;
            _authUtils = authUtils;
        }




        [HttpGet("Owner")]

        public async Task<ActionResult<List<Trip>>> GetUserTrips()
        {

            AppUser? user = _authUtils.GetLoggedInUser(HttpContext);
            if (user == null)
                return BadRequest("the user try to join the trip is not found");
            var trips = await _context.Trips
                  
                .Where(t => t.OwnerId == user.Id)
                 .Include(p => p.Passangers)
                .Include(o => o.Car)
                .Include(o => o.Owner) 
               .Include(o => o.FromCity)
               .Include(o => o.ToCity)
                       
               .ToListAsync();

            return Ok(trips);   
        }


        [HttpGet("Joined")]

        public async Task<ActionResult<List<Trip>>> GetUserJoinedTrips()
        {

            AppUser? user = _authUtils.GetLoggedInUser(HttpContext);
            if (user == null)
                return BadRequest("the user try to join the trip is not found");
            var trips = await _context.Trips
               .Include(o => o.Owner)
               .Include(o => o.Car)
               .Include(o => o.FromCity)
               .Include(o => o.ToCity)
               .ToListAsync();

            List<Trip> joinedTrips = new();

            foreach (var trip in trips)
            {
                foreach(var passanger in trip.Passangers)
                {
                    if(passanger.Id == user.Id)
                    {
                        joinedTrips.Add(trip);
                    }
                }
            }

            return Ok(joinedTrips);
        }


        [HttpPost]
        public async Task<ActionResult<Trip>> CreateTip([FromBody] CreateTripDto tripDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            AppUser? owner = _authUtils.GetLoggedInUser(HttpContext);

            if(owner == null) { return BadRequest("Owner cannot be null"); }


            City? fromCity = await _context.Cities.Where(c => c.Id == tripDto.FromCityId).FirstOrDefaultAsync();

           

            City? toCity = await _context.Cities.Where(c => c.Id == tripDto.ToCityId).FirstOrDefaultAsync();
            if(toCity is null || fromCity is null) return BadRequest(ModelState);
                    
            Car? car = await _context.Cars.FindAsync(tripDto.CarId);

            if(car is null)
            {
                return BadRequest(tripDto);
            }

            var trip = new Trip()
            {
                OwnerId = owner.Id,
                Owner = owner,
                FromCity = fromCity,
                ToCity = toCity,
                ToCityId = toCity.Id,
                FromCityId = fromCity.Id,
                TripPrice = tripDto.TripPrice,
                TotalSeats = tripDto.TotalSeats,
                RemainingSeats = tripDto.TotalSeats,
                TripDate = tripDto.TripDate,
                Status = TripStatus.OPEN.ToString(),
                Car = car,
                CardId = car.Id
            };

            await _context.Trips.AddAsync(trip);
            await _context.SaveChangesAsync();

            return Ok(trip);
        }


        [HttpGet("To")]
        public async Task<ActionResult<List<Trip>>> GetTripByDestinationCity([FromQuery] string destinationCity)
        {

            var trips = await _context.Trips  
                .Include(o => o.Owner)
                .Include(o => o.FromCity)
                .Include(o => o.ToCity)
                .Where(t => t.ToCity.CityName.ToLower().Trim() == destinationCity.ToLower().Trim())
               
                .ToListAsync();

            return Ok(trips);
        }


        [HttpGet("From")]
        public async Task<ActionResult<List<Trip>>> GetTripByFromCity([FromQuery] string fromCityName)
        {
            var trips = await _context.Trips
                .Include(o => o.Owner)
                .Include(o => o.FromCity)
                .Include(o => o.ToCity)
                .Where(t => t.FromCity.CityName.ToLower().Trim() == fromCityName.ToLower().Trim())
                .Include(p => p.Passangers)
                .ToListAsync();

            return Ok(trips);
        }




        [HttpPut("Close")]
        public async Task<ActionResult<Trip>> CloseTrip([FromRoute] Guid tripId)
        {
            var trip = await _context.Trips
               .Where(t => t.Id == tripId)
              .Include(o => o.Owner)
              .Include(o => o.FromCity)
              .Include(o => o.ToCity)
            
              .FirstOrDefaultAsync();
            if (trip == null)
                return BadRequest("Trip not found");

            trip.Status = TripStatus.CLOSED.ToString();
        
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();

            return Ok(trip);

        }


        [HttpPut("Cancel")]
        public async Task<ActionResult<Trip>> Cancel([FromRoute] Guid tripId)
        {
            var trip = await _context.Trips
               .Where(t => t.Id == tripId)
              .Include(o => o.Owner)
              .Include(o => o.FromCity)
              .Include(o => o.ToCity)
            
              .FirstOrDefaultAsync();
            if (trip == null)
                return BadRequest("Trip not found");

            trip.Status = TripStatus.CANCLED.ToString();

            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();

            return Ok(trip);

        }




        [HttpPut("{tripId}")]
        public async Task<ActionResult<List<Trip>>> JoinTrip([FromRoute] Guid tripId)
        {
            var trip = await _context.Trips
                .Where(t => t.Id == tripId)
               .Include(o => o.Owner)
               .Include(o => o.FromCity)
               .Include(o => o.ToCity)
            
               .FirstOrDefaultAsync();

            if (trip == null)
                return BadRequest("Trip not found");

            if (trip.Status != TripStatus.OPEN.ToString())
                return BadRequest("you can join only opened trips");

            if (trip.RemainingSeats <= 0)
                return BadRequest("You cannot join this trip");


            AppUser? user = _authUtils.GetLoggedInUser(HttpContext);
            if (user == null)
                return BadRequest("the user try to join the trip is not found");


            trip.RemainingSeats -= 1;
            trip.Passangers.Add(user);
             _context.Trips.Update(trip);
            await _context.SaveChangesAsync();

            return Ok(trip);

        }




        [HttpGet("query")]
        public async Task<ActionResult<List<Trip>>> QueryTrip([FromQuery] string fromCityName, [FromQuery] string destinationCity)
        {
            var trips = await _context.Trips
                .Include(o => o.Owner)
                .Include(o => o.FromCity)
                .Include(o => o.ToCity)
                .Where(t => t.FromCity.CityName.ToLower().Trim() == fromCityName.ToLower().Trim() && t.ToCity.CityName.ToLower().Trim() == destinationCity.ToLower().Trim())
                .Include(p => p.Passangers)
                .ToListAsync();

            return Ok(trips);
        }


    }
}
