using DiniM3ak.Data;
using DiniM3ak.Entity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DiniM3ak.Utils
{
    public class AuthUtils
    {


        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthUtils(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public AppUser? GetLoggedInUser(HttpContext context)
        {
           
            string? userId = context.User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            if(userId == null)
            {
                return null;
            }

            AppUser? user = _context.Users.Where(u => u.Id == Guid.Parse(userId)).FirstOrDefault();

            if(user == null)
            {
                return null;
            }

            return user;
        }


        public string GenerateToken(AppUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
             _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


       



    }
}
