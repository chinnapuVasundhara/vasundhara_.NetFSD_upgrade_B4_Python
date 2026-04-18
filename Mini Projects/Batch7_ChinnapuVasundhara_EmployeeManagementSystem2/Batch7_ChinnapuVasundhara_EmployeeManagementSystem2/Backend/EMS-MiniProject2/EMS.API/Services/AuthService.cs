using EMS.API.Data;
using EMS.API.DTOs;
using EMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS.API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResponseDto> RegisterAsync(AuthRequestDto dto)
        {
            // Enforce unique username
            if (await _context.AppUsers.AnyAsync(u => u.Username.ToLower() == dto.Username.ToLower()))
                return new AuthResponseDto { Success = false, Message = "Username already exists." };

            var user = new AppUser
            {
                Username = dto.Username,
                // Hash password before saving to DB
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role == "Admin" ? "Admin" : "Viewer"
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponseDto { Success = true, Message = "Registration successful." };
        }

        public async Task<AuthResponseDto> LoginAsync(AuthRequestDto dto)
        {
            var user = await _context.AppUsers.SingleOrDefaultAsync(u => u.Username == dto.Username);

            // Verify BCrypt hash
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return new AuthResponseDto { Success = false, Message = "Invalid credentials." };

            return new AuthResponseDto
            {
                Success = true,
                Username = user.Username,
                Role = user.Role,
                Token = GenerateToken(user),
                Message = "Login successful."
            };
        }

        private string GenerateToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(double.Parse(_config["Jwt:ExpiryHours"] ?? "8")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}