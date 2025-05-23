using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project.DTOs;
using Mock_Project.Models;
using Mock_Project.Repositories;

namespace Mock_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRequestRepository _loginRequestRepository;

        public LoginController(ILoginRequestRepository loginRequestRepository)
        {
            _loginRequestRepository = loginRequestRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LoginRequestInputDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string roleToAssign = "viewer";

            // Check if the user requested admin role
            if (!string.IsNullOrEmpty(model.Role) && model.Role.ToLower() == "admin")
            {
                // Check if an admin already exists
                var existingAdmin = await _loginRequestRepository.GetAdminAsync();
                if (existingAdmin == null)
                {
                    roleToAssign = "admin";
                }
                // else: keep as viewer, since admin already exists
            }

            var loginRequest = new LoginRequest
            {
                Username = model.Username,
                Password = model.Password,
                Role = roleToAssign
            };

            var created = await _loginRequestRepository.AddAsync(loginRequest);

            var result = new LoginRequestDto
            {
                Id = created.Id,
                Username = created.Username,
                // Do NOT return password in production!
                Password = created.Password,
                Role = created.Role
            };

            return Ok(result);
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequestInputDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _loginRequestRepository.GetByUsernameAsync(model.Username);

            if (user == null || user.Password != model.Password)
                return Unauthorized(new { message = "Invalid username or password" });

            var result = new LoginRequestDto
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Role = user.Role
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _loginRequestRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = "Login request not found." });

            await _loginRequestRepository.DeleteAsync(id);
            return NoContent();
        }


    }
}
