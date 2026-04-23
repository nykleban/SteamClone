using Microsoft.AspNetCore.Mvc;
using SteamClone.API.Extensions;
using SteamClone.BLL.Dtos.Auth;
using SteamClone.BLL.Services;

namespace SteamClone.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto dto)
        {
            //_logger.LogInformation($"[{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}] - User login\n\tusername: {dto.UserName}\n\tpassword: {dto.Password}");
            var response = await _authService.LoginAsync(dto);
            return this.GetResult(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto dto)
        {
            var response = await _authService.RegisterAsync(dto);
            return this.GetResult(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string token)
        {
            var response = await _authService.ConfirmEmailAsync(userId, token);
            return this.GetResult(response);
        }
    }
}
