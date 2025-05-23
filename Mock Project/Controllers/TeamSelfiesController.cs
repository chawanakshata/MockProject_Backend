using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project.DTOs;
using Mock_Project.Models;
using Mock_Project.Repositories;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Mock_Project.Exceptions;

namespace Mock_Project.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TeamSelfiesController : ControllerBase
    {
        private readonly ITeamSelfieRepository _teamSelfieRepository;
        private readonly IFileConverterService _fileConverterService;

        public TeamSelfiesController(ITeamSelfieRepository teamSelfieRepository, IFileConverterService fileConverterService)
        {
            _teamSelfieRepository = teamSelfieRepository;
            _fileConverterService = fileConverterService;
        }

        // Handles file upload for a team selfie.
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] CreateFileUploadRequest model)
        {
            if (model.File == null || model.File.Length == 0)
                throw new BadRequestException("No file uploaded.");

            string base64Image = await _fileConverterService.ConvertToBase64Async(model.File);

            var teamSelfie = new TeamSelfie // Creates a new TeamSelfie object
            {
                TeamMemberName = model.TeamMemberName,
                Base64Image = base64Image
            };
            await _teamSelfieRepository.AddAsync(teamSelfie); // Adds the new TeamSelfie object to the repository

            var teamSelfieDto = new TeamSelfieDto // Creates a DTO to return the created TeamSelfie object
            {
                Id = teamSelfie.Id,
                TeamMemberName = teamSelfie.TeamMemberName,
                Base64Image = teamSelfie.Base64Image
            };
            return Ok(teamSelfieDto); // Returns the status code 200 OK with the created TeamSelfie DTO
        }


        // Handles PUT requests for updating a team selfie by ID.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeamSelfie(int id, [FromForm] CreateFileUploadRequest model)
        {
            var teamSelfie = await _teamSelfieRepository.GetByIdAsync(id);
            if (teamSelfie == null)
                throw new NotFoundException("Team selfie not found.");

            if (model.File != null && model.File.Length > 0)
            {
                // Use the file converter service to convert the file to base64
                teamSelfie.Base64Image = await _fileConverterService.ConvertToBase64Async(model.File);
                
            }
            teamSelfie.TeamMemberName = model.TeamMemberName;
            await _teamSelfieRepository.UpdateAsync(teamSelfie);
            return NoContent();
        }

        // Handles GET requests for retrieving all team selfies.
        [HttpGet]
        public async Task<IActionResult> GetAllTeamSelfies()
        {
            var teamSelfies = await _teamSelfieRepository.GetAllAsync();
            var teamSelfieDtos = teamSelfies.Select(ts => new TeamSelfieDto
            {
                Id = ts.Id,
                TeamMemberName = ts.TeamMemberName,
                Base64Image = ts.Base64Image
            });

            return Ok(teamSelfieDtos);
        }

        // Handles GET requests for retrieving a team selfie by ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamSelfieById(int id)
        {
            var teamSelfie = await _teamSelfieRepository.GetByIdAsync(id);
            if (teamSelfie == null)
                throw new NotFoundException("Team selfie not found.");

            var teamSelfieDto = new TeamSelfieDto
            {
                Id = teamSelfie.Id,
                TeamMemberName = teamSelfie.TeamMemberName,
                Base64Image = teamSelfie.Base64Image
            };

            return Ok(teamSelfieDto);
        }


        // Handles DELETE requests for deleting a team selfie by ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamSelfie(int id)
        {
            var teamSelfie = await _teamSelfieRepository.GetByIdAsync(id);
            if (teamSelfie == null)
                throw new NotFoundException("Team selfie not found.");

            await _teamSelfieRepository.DeleteAsync(id);

            return NoContent();
        }


    }
}






