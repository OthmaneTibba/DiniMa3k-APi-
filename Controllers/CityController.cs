using DiniM3ak.Data;
using DiniM3ak.Dtos.City;
using DiniM3ak.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiniM3ak.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CityController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost]
       public async Task<ActionResult<City>> CreateCity([FromBody] CreateCityDto cityDto)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var city = new City() { CityName = cityDto.CityName };

            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            return Ok(city);

        }

        [HttpGet]
        public async Task<ActionResult<List<City>>> GetAll() => Ok(await _context.Cities.ToListAsync());

        [HttpGet("{cityId}")]
        public async Task<ActionResult<City>> GetCityById(Guid cityId) => Ok(await _context.Cities.FindAsync(cityId));
    }
}
