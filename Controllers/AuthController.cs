using CloudinaryDotNet.Actions;
using DiniM3ak.Data;
using DiniM3ak.Dtos.Auth;
using DiniM3ak.Entity;
using DiniM3ak.Services;
using DiniM3ak.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DiniM3ak.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly AuthUtils _authUtils;
        private readonly PictureService _pictureService;

        public AuthController(AppDbContext context, AuthUtils authUtils , PictureService pictureService)
        {
            _context = context;
            _authUtils = authUtils;
            _pictureService = pictureService;
        }



        [HttpPost("Register")]
        public async Task<ActionResult<AppUser>> Register([FromForm] AuthRegisterRequestDto authRequestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users
                .Where(u => u.Username == authRequestDto.Fullname || u.Email == authRequestDto.Email)
                .FirstOrDefaultAsync();


            if(user is not null)
            {
                return BadRequest("Username or email already exist");
            }


            CreatePasswordHash(password: authRequestDto.Password, out byte[] PasswordHash, out byte[] PasswordSalt);


            var newUser = new AppUser()
            {
                Username = authRequestDto.Fullname,
                Email = authRequestDto.Email,
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt,
                ProfilePicture = _pictureService.UaploadProdilePicture(authRequestDto.ProfilePicture)
            };


           await _context.Users.AddAsync(newUser);   
           await _context.SaveChangesAsync();


            return Ok("User Registred Successfully");
        }



        [HttpPost("Login")]
        public async Task<ActionResult<AppUser>> Login([FromBody] AuthLoginRequestDto authLoginRequestDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                .Where(u => u.Username == authLoginRequestDto.Username || u.Email == authLoginRequestDto.Username)
                .FirstOrDefaultAsync();

            if (user is null)
                return BadRequest("User not found");

            var verifyPassword = VerifyPasswordHash(authLoginRequestDto.Password , user.PasswordHash, user.PasswordSalt);

            if (!verifyPassword)
                return BadRequest("Wrong crediantials");

            return Ok(new AuthTokenResponse() { AccessToken = _authUtils.GenerateToken(user)});
        }

        [NonAction]
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [NonAction]
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
