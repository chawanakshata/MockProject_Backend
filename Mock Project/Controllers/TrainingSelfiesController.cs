
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project.DTOs;
using Mock_Project.Models;
using Mock_Project.Repositories;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mock_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingSelfiesController : ControllerBase
    {
        private readonly ITrainingSelfieRepository _trainingSelfieRepository;
        private readonly IWebHostEnvironment _environment;

        public TrainingSelfiesController(ITrainingSelfieRepository trainingSelfieRepository, IWebHostEnvironment environment)
        {
            _trainingSelfieRepository = trainingSelfieRepository;
            _environment = environment;
        }

        // Creates a new training selfie from the provided data.
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest1 model)
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

            var trainingSelfie = new TrainingSelfie
            {
                PersonName = model.PersonName,
                ImageUrl = $"/uploads/{model.File.FileName}"
            };

            await _trainingSelfieRepository.AddAsync(trainingSelfie);

            var trainingSelfieDto = new TrainingSelfieDto
            {
                Id = trainingSelfie.Id,
                PersonName = trainingSelfie.PersonName,
                ImageUrl = trainingSelfie.ImageUrl
            };

            return Ok(trainingSelfieDto);
        }

        // Retrieves all training selfies.
        [HttpGet]
        public async Task<IActionResult> GetAllTrainingSelfies()
        {
            var trainingSelfies = await _trainingSelfieRepository.GetAllAsync();
            var trainingSelfieDtos = trainingSelfies.Select(ts => new TrainingSelfieDto
            {
                Id = ts.Id,
                PersonName = ts.PersonName,
                ImageUrl = ts.ImageUrl
            });

            return Ok(trainingSelfieDtos);
        }

        // Retrieves a specific training selfie by ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainingSelfieById(int id)
        {
            var trainingSelfie = await _trainingSelfieRepository.GetByIdAsync(id);
            if (trainingSelfie == null)
                return NotFound("Training selfie not found.");

            var trainingSelfieDto = new TrainingSelfieDto
            {
                Id = trainingSelfie.Id,
                PersonName = trainingSelfie.PersonName,
                ImageUrl = trainingSelfie.ImageUrl

            };

            return Ok(trainingSelfieDto);
        }

        // Updates an existing training selfie by its id with the new data.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingSelfie(int id, [FromForm] FileUploadRequest1 model)
        {
            var trainingSelfie = await _trainingSelfieRepository.GetByIdAsync(id);
            if (trainingSelfie == null)
                return NotFound("Training selfie not found.");

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

                trainingSelfie.ImageUrl = $"/uploads/{model.File.FileName}";
            }

            trainingSelfie.PersonName = model.PersonName;

            await _trainingSelfieRepository.UpdateAsync(trainingSelfie);

            return NoContent();
        }

        // Deletes a training selfie by its id.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingSelfie(int id)
        {
            var trainingSelfie = await _trainingSelfieRepository.GetByIdAsync(id);
            if (trainingSelfie == null)
                return NotFound("Training selfie not found.");

            await _trainingSelfieRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
