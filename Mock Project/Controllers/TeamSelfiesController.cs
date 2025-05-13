using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project.Data;
using Mock_Project.DTOs;
using Mock_Project.Models;
using Mock_Project.Repositories;

namespace Mock_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamSelfiesController : ControllerBase
    {
        private readonly ITeamSelfieRepository _teamSelfieRepository;
        private readonly IWebHostEnvironment _environment;

        public TeamSelfiesController(ITeamSelfieRepository teamSelfieRepository, IWebHostEnvironment environment)
        {
            _teamSelfieRepository = teamSelfieRepository;
            _environment = environment;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] CreateFileUploadRequest model)
        {
            // Handles file upload for a team selfie.
            // Saves the uploaded file to the server, creates a TeamSelfie record, and returns the created DTO.
            
            if (model.File == null || model.File.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, model.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            var teamSelfie = new TeamSelfie
            {
                TeamMemberName = model.TeamMemberName,
                ImageUrl = $"/uploads/{model.File.FileName}",

            };

            await _teamSelfieRepository.AddAsync(teamSelfie);

            var teamSelfieDto = new TeamSelfieDto
            {
                Id = teamSelfie.Id,
                TeamMemberName = teamSelfie.TeamMemberName,
                ImageUrl = teamSelfie.ImageUrl,
            };

            return Ok(teamSelfieDto);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeamSelfie(int id, [FromForm] CreateFileUploadRequest model)
        {
            // Updates an existing team selfie record.
            // If a new file is uploaded, replaces the old image. Updates the team member name.

            var teamSelfie = await _teamSelfieRepository.GetByIdAsync(id);
            if (teamSelfie == null)
                return NotFound("Team selfie not found.");

            if (model.File != null && model.File.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, model.File.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                teamSelfie.ImageUrl = $"/uploads/{model.File.FileName}";
            }

            teamSelfie.TeamMemberName = model.TeamMemberName;

            await _teamSelfieRepository.UpdateAsync(teamSelfie);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeamSelfies()
        {
            //Retrieves all team selfie records and returns them as DTOs.

            var teamSelfies = await _teamSelfieRepository.GetAllAsync();
            var teamSelfieDtos = teamSelfies.Select(ts => new TeamSelfieDto
            {
                Id = ts.Id,
                TeamMemberName = ts.TeamMemberName,
                ImageUrl = ts.ImageUrl,
            });

            return Ok(teamSelfieDtos);
        }

   
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamSelfieById(int id)
        {
            //Retrieves a single team selfie by its ID and returns it as a DTO.

            var teamSelfie = await _teamSelfieRepository.GetByIdAsync(id);
            if (teamSelfie == null)
                return NotFound("Team selfie not found.");

            var teamSelfieDto = new TeamSelfieDto
            {
                Id = teamSelfie.Id,
                TeamMemberName = teamSelfie.TeamMemberName,
                ImageUrl = teamSelfie.ImageUrl,
            };

            return Ok(teamSelfieDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamSelfie(int id)
        {
            //Deletes a team selfie record by its ID.

            var teamSelfie = await _teamSelfieRepository.GetByIdAsync(id);
            if (teamSelfie == null)
                return NotFound("Team selfie not found.");

            await _teamSelfieRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
