using DiniM3ak.Data;
using DiniM3ak.Dtos.Car;
using DiniM3ak.Entity;
using DiniM3ak.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiniM3ak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthUtils _authUtils;

        public CarController(AppDbContext context, AuthUtils authUtils)
        {
            _context = context;
            _authUtils = authUtils;
        }



        [HttpPost]
        public async Task<ActionResult<Car>> CreateCar([FromBody] CreateCarDto carDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            AppUser? user = _authUtils.GetLoggedInUser(HttpContext);

            if (user == null)
            {
                return BadRequest("Logged user cannot be null");
            }

            Car car = new Car()
            { 
                CarModel = carDto.CarModel,
                CarName = carDto.CarName,
                MaxSeatNumber = carDto.MaxSeatNumber,
                User = user,
                UserId = user.Id
            };

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            return Ok(car); 

        }




        [HttpGet]
        public async Task<ActionResult<List<Car>>> GetUserCars()
        {
            AppUser? user = _authUtils.GetLoggedInUser(HttpContext);

            if (user == null)
            {
                return BadRequest("Logged user cannot be null");
            }

            return Ok(await _context.Cars
                .Where(u => u.UserId == user.Id)
                .ToListAsync());

        }



        [HttpDelete("{carId}")]
        public async Task<ActionResult> DeleteCar([FromRoute] Guid carId)
        {
            Car? car= await _context.Cars.FindAsync(carId);
            if (car == null)
                return NotFound();
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
