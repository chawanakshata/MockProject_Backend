using Microsoft.AspNetCore.Mvc;
using Mock_Project.DTOs;
using Mock_Project.Models;
using Mock_Project.Repositories;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Mock_Project.DTOs.LoginRequest loginRequest) // Fully qualify the type to resolve ambiguity
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _authRepository.LoginAsync(loginRequest.Username, loginRequest.Password);
        if (user == null)
            return Unauthorized(new { message = "Invalid username or password." });

        return Ok(new
        {
            message = "Login successful",
            userId = user.Id,
            username = user.Name,
            role = user.Role // Include role in response
        });
    }

    [HttpGet("login")]
    public async Task<IActionResult> GetUserByCredentials([FromQuery] string username, [FromQuery] string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return BadRequest(new { message = "Username and password are required." });

        var user = await _authRepository.LoginAsync(username, password);

        if (user == null)
            return NotFound(new { message = "User not found or invalid credentials." });

        // Return only safe info, not the password!
        return Ok(new
        {
            userId = user.Id,
            username = user.Name,
            role = user.Role
        });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] Mock_Project.DTOs.LoginRequest loginRequest) // Fully qualify the type to resolve ambiguity
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _authRepository.RegisterAsync(loginRequest.Username, loginRequest.Password);
            return Ok(new { message = "Signup successful" });
        }
        catch (Exception ex)
        {
            // Return the inner exception message if it exists, otherwise the main exception message
            return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
        }
    }
}
