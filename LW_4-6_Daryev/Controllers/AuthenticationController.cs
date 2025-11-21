using System.Security.Claims;
using LW_4_3_5_Daryev_PI231.JWT_Manager;
using LW_4_3_5_Daryev_PI231.Models;
using LW_4_3_5_Daryev_PI231.Services;
using Microsoft.AspNetCore.Mvc;

namespace LW_4_3_5_Daryev_PI231.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JWTTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationController(IUserService userService, JWTTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var existingUser = await _userService.GetByEmailAsync(user.Email);
            if (existingUser != null)
                return BadRequest("User already exist");

            user.PasswordHash = _passwordHasher.HashPassword(user.PasswordHash);
            await _userService.CreateAsync(user);

            return Ok("User registered");
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] User model)
        {
            var user = await _userService.GetByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized("Incorrect login or passsword");

            if (user.PasswordHash != _passwordHasher.HashPassword(model.PasswordHash))
                return Unauthorized("Incorrect password");

            var token = _jwtTokenGenerator.Generate(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userService.UpdateAsync(user);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken,
                TokenExpiryTime = DateTime.UtcNow.AddMinutes(60),
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
            });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromHeader(Name = "Authorization")] string authHeader, [FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return BadRequest("Missing or invalid Authorization header");

            var token = authHeader.Substring("Bearer ".Length);

            var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(token);

            if (principal == null)
                return BadRequest("Invalid refresh token");

            var id = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (id == null) throw new Exception("Invalid token claims");

            var user = await _userService.GetAsync(id);

            if (user == null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid refresh token");
            }

            var newAccessToken = _jwtTokenGenerator.Generate(user);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userService.UpdateAsync(user);

            return Ok(new
            {
                token = newAccessToken,
                refreshToken = newRefreshToken,
                TokenExpiryTime = DateTime.UtcNow.AddMinutes(60),
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
            });
        }

    }
}
