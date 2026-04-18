using EMS.API.DTOs;
using EMS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Anyone can hit these endpoints
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result.Success) return Conflict(new { message = result.Message });
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (!result.Success) return Unauthorized(new { message = result.Message });
            return Ok(result);
        }
    }
}