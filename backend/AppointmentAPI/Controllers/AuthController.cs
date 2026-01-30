using AppointmentAPI.Data;
using AppointmentAPI.Models;
using AppointmentAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace AppointmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        // LOGIN MUST BE ANONYMOUS
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginDto user)
        {
            if (user == null)
            return Unauthorized("Request body is empty");

            var dbUser = _context.Users
                .FirstOrDefault(u =>
                    u.Username == user.Username &&
                    u.Password == user.Password);

            if (dbUser == null)
                return Unauthorized("Invalid credentials");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dbUser.Username),
                new Claim(ClaimTypes.Role, dbUser.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}

