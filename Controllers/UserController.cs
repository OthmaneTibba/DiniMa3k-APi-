using DiniM3ak.Entity;
using DiniM3ak.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiniM3ak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly AuthUtils _authUtils;

        public UserController(AuthUtils authUtils)
        {
            _authUtils = authUtils;
        }


        [HttpGet]
        public async Task<ActionResult<AppUser>> Get()
        {
            AppUser? user = _authUtils.GetLoggedInUser(HttpContext);
            if(user == null) { return BadRequest("User not found"); }
            return Ok(user);
        }
    }
}
