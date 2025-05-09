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
            var teamSelfie = await _teamSelfieRepository.GetByIdAsync(id);
            if (teamSelfie == null)
                return NotFound("Team selfie not found.");

            await _teamSelfieRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}


//-------------------------------

//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Mock_Project.Data;
//using Mock_Project.DTOs;
//using Mock_Project.Models;
//using Mock_Project.Repositories;

//namespace Mock_Project.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TeamSelfiesController : ControllerBase
//    {
//        private readonly ITeamSelfieRepository _teamSelfieRepository;
//        private readonly IWebHostEnvironment _environment;

//        public TeamSelfiesController(ITeamSelfieRepository teamSelfieRepository, IWebHostEnvironment environment)
//        {
//            _teamSelfieRepository = teamSelfieRepository;
//            _environment = environment;
//        }

//        [HttpPost("UploadFile")]
//        public async Task<IActionResult> UploadFile([FromForm] CreateFileUploadRequest model)
//        {
//            if (model.File == null || model.File.Length == 0)
//                return BadRequest("No file uploaded.");

//            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
//            if (!Directory.Exists(uploadsFolder))
//                Directory.CreateDirectory(uploadsFolder);

//            var filePath = Path.Combine(uploadsFolder, model.File.FileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await model.File.CopyToAsync(stream);
//            }

//            var teamSelfie = new TeamSelfie
//            {
//                TeamMemberName = model.TeamMemberName,
//                ImageUrl = $"/uploads/{model.File.FileName}",
//                Designation = model.Designation
//            };

//            await _teamSelfieRepository.AddAsync(teamSelfie);

//            var teamSelfieDto = new TeamSelfieDto
//            {
//                Id = teamSelfie.Id,
//                TeamMemberName = teamSelfie.TeamMemberName,
//                ImageUrl = teamSelfie.ImageUrl,
//                Designation = teamSelfie.Designation
//            };

//            return Ok(teamSelfieDto);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateTeamSelfie(int id, [FromForm] CreateFileUploadRequest model)
//        {
//            var teamSelfie = await _teamSelfieRepository.GetByIdAsync(id);
//            if (teamSelfie == null)
//                return NotFound("Team selfie not found.");

//            if (model.File != null && model.File.Length > 0)
//            {
//                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
//                if (!Directory.Exists(uploadsFolder))
//                    Directory.CreateDirectory(uploadsFolder);

//                var filePath = Path.Combine(uploadsFolder, model.File.FileName);

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await model.File.CopyToAsync(stream);
//                }

//                teamSelfie.ImageUrl = $"/uploads/{model.File.FileName}";
//            }

//            teamSelfie.TeamMemberName = model.TeamMemberName;
//            teamSelfie.Designation = model.Designation;

//            await _teamSelfieRepository.UpdateAsync(teamSelfie);

//            return NoContent();
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllTeamSelfies()
//        {
//            var teamSelfies = await _teamSelfieRepository.GetAllAsync();
//            var teamSelfieDtos = teamSelfies.Select(ts => new TeamSelfieDto
//            {
//                Id = ts.Id,
//                TeamMemberName = ts.TeamMemberName,
//                ImageUrl = ts.ImageUrl,
//                Designation = ts.Designation
//            });

//            return Ok(teamSelfieDtos);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetTeamSelfieById(int id)
//        {
//            var teamSelfie = await _teamSelfieRepository.GetByIdAsync(id);
//            if (teamSelfie == null)
//                return NotFound("Team selfie not found.");

//            var teamSelfieDto = new TeamSelfieDto
//            {
//                Id = teamSelfie.Id,
//                TeamMemberName = teamSelfie.TeamMemberName,
//                ImageUrl = teamSelfie.ImageUrl,
//                Designation = teamSelfie.Designation
//            };

//            return Ok(teamSelfieDto);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTeamSelfie(int id)
//        {
//            var teamSelfie = await _teamSelfieRepository.GetByIdAsync(id);
//            if (teamSelfie == null)
//                return NotFound("Team selfie not found.");

//            await _teamSelfieRepository.DeleteAsync(id);

//            return NoContent();
//        }
//    }
//}
