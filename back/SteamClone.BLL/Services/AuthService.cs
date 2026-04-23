
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SteamClone.BLL.Dtos.Auth;
using SteamClone.BLL.Settings;
using SteamClone.DAL.Entities;
using System.Net;
namespace SteamClone.BLL.Services
{
    public class AuthService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly JwtService _jwtService;
        private readonly IEmailSender _emailSender;
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<UserEntity> userManager,
            JwtService jwtService,
            IEmailSender emailSender,
            IOptions<EmailSettings> emailOptions, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _emailSender = emailSender;
            _emailSettings = emailOptions.Value;
            _logger = logger;
        }

        public async Task<ServiceResponse> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user == null)
            {
                _logger.LogWarning("[{Date}] - Login attempt failed for username: {UserName}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), dto.UserName);
                return ServiceResponse.Error($"Користувача з іменем '{dto.UserName}' не існує");
            }

            bool passwordResult = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordResult)
            {
                return ServiceResponse.Error("Невірний пароль");
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Jwt token
            var token = _jwtService.GetAcessToken(user, roles);

            return ServiceResponse.Success("Успішний вхід", token);
        }

        public async Task<ServiceResponse> RegisterAsync(RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName))
            {
                return ServiceResponse.Error("Ім'я користувача є обов'язковим");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return ServiceResponse.Error("Email є обов'язковим");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return ServiceResponse.Error("Пароль є обов'язковим");
            }

            if (dto.Password.Length < 6)
            {
                return ServiceResponse.Error("Пароль повинен містити щонайменше 6 символів");
            }

            var existingUserByName = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUserByName != null)
            {
                return ServiceResponse.Error($"Користувач з іменем '{dto.UserName}' вже існує");
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUserByEmail != null)
            {
                return ServiceResponse.Error($"Користувач з email '{dto.Email}' вже існує");
            }

            var user = new UserEntity
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Image = dto.Image
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                return ServiceResponse.Error(errors);
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, "user");
            if (!addToRoleResult.Succeeded)
            {
                var errors = string.Join("; ", addToRoleResult.Errors.Select(e => e.Description));
                return ServiceResponse.Error(errors);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GetAcessToken(user, roles);
            await SendConfirmationEmailAsync(user);

            return ServiceResponse.Success("Реєстрація успішна. Пошту ще потрібно підтвердити", token);
        }

        public async Task<ServiceResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResponse.Error("Користувача не знайдено");
            }

            var decodedToken = WebUtility.UrlDecode(token);
            var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!confirmResult.Succeeded)
            {
                var errors = string.Join("; ", confirmResult.Errors.Select(e => e.Description));
                return ServiceResponse.Error(errors);
            }

            return ServiceResponse.Success("Пошту успішно підтверджено");
        }

        private async Task SendConfirmationEmailAsync(UserEntity user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(_emailSettings.ConfirmEmailUrl))
            {
                return;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var confirmationLink = $"{_emailSettings.ConfirmEmailUrl}?userId={user.Id}&token={encodedToken}";

            var message = $"""
                <h2>Підтвердження пошти</h2>
                <p>Щоб підтвердити вашу пошту, перейдіть за посиланням:</p>
                <p><a href="{confirmationLink}">Підтвердити пошту</a></p>
                """;

            await _emailSender.SendEmailAsync(user.Email, "Підтвердження пошти", message);
        }
    }
}
