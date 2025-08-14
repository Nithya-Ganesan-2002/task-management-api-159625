using dotnet.DTOs.Auth;
using dotnet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet.Controllers
{
    /// <summary>
    /// Authentication endpoints for TaskFlow API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // PUBLIC_INTERFACE
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user and receive a JWT token.
        /// </summary>
        /// <param name="request">Registration payload containing username, email and password.</param>
        /// <returns>JWT token and user info upon success.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result is null) return BadRequest(new { message = "UserName or Email already exists." });
            return Ok(result);
        }

        /// <summary>
        /// Login with username or email to obtain a JWT token.
        /// </summary>
        /// <param name="request">Login payload containing username/email and password.</param>
        /// <returns>JWT token and user info upon success.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (result is null) return Unauthorized(new { message = "Invalid credentials." });
            return Ok(result);
        }
    }
}
