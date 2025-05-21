using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project.DTOs;
using Mock_Project.Exceptions;
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
  
        public TrainingSelfiesController(ITrainingSelfieRepository trainingSelfieRepository)
        {
            _trainingSelfieRepository = trainingSelfieRepository;
        }

        // Handles POST requests for uploading a training selfie.
        [HttpPost("UploadFile")]
        [ProducesResponseType(typeof(TrainingSelfie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest1 model) 
        {
            if (model.File == null || model.File.Length == 0)
                throw new BadRequestException("No file uploaded.");

            // Convert uploaded file to Base64 string
            string base64String;
            using (var memoryStream = new MemoryStream())  // Creates a memory stream to temporarily hold the file data
            {
                await model.File.CopyToAsync(memoryStream); // Copies the uploaded file data to the memory stream
                var fileBytes = memoryStream.ToArray(); // Converts the memory stream to a byte array
                base64String = Convert.ToBase64String(fileBytes); // Converts the byte array to a base64 string
            }

            var trainingSelfie = new TrainingSelfie // Creates a new TrainingSelfie object to store the selfie data
            {
                PersonName = model.PersonName,
                Base64Image = base64String
            };

            await _trainingSelfieRepository.AddAsync(trainingSelfie); // Save to database

            return Ok(trainingSelfie); // Returns the created TrainingSelfie object as a response with status code 200 OK
        }


        // Retrieves all training selfies
        [HttpGet]
        public async Task<IActionResult> GetAllTrainingSelfies()
        {
            var trainingSelfies = await _trainingSelfieRepository.GetAllAsync(); // Fetches all training selfies from the repository

            var trainingSelfieDtos = trainingSelfies.Select(ts => new TrainingSelfieDto  
            {
                Id = ts.Id,
                PersonName = ts.PersonName,
                Base64Image = ts.Base64Image
            }).ToList();  // Maps the TrainingSelfie objects to TrainingSelfieDto objects

            return Ok(trainingSelfieDtos); // Returns the list of TrainingSelfieDto objects as a response with status code 200 OK
        }

        // Retrieves a specific training selfie by ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainingSelfieById(int id)
        {
            var trainingSelfie = await _trainingSelfieRepository.GetByIdAsync(id); // Fetches the training selfie record by ID
            if (trainingSelfie == null)
                throw new NotFoundException("Training selfie not found.");

            var trainingSelfieDto = new TrainingSelfieDto  // Maps the TrainingSelfie object to a TrainingSelfieDto object
            {
                Id = trainingSelfie.Id,
                PersonName = trainingSelfie.PersonName,
                Base64Image = trainingSelfie.Base64Image
            };

            return Ok(trainingSelfieDto); // Returns the TrainingSelfieDto object as a response with status code 200 OK
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingSelfie(int id, [FromForm] FileUploadRequest1 model)
        {
            var trainingSelfie = await _trainingSelfieRepository.GetByIdAsync(id); // Fetches the training selfie record by ID
            if (trainingSelfie == null)
                throw new NotFoundException("Training selfie not found.");

            trainingSelfie.PersonName = model.PersonName; // Update the name

            if (model.File != null && model.File.Length > 0)
            {
                using (var memoryStream = new MemoryStream()) // Creates a memory stream to temporarily hold the file data
                {                  
                    await model.File.CopyToAsync(memoryStream); // Copies the uploaded file data to the memory stream
                    var fileBytes = memoryStream.ToArray(); // Converts the memory stream to a byte array
                    trainingSelfie.Base64Image = Convert.ToBase64String(fileBytes); // Converts the byte array to a base64 string
                }
            }
            await _trainingSelfieRepository.UpdateAsync(trainingSelfie);

            var trainingSelfieDto = new TrainingSelfieDto  // Maps the updated TrainingSelfie object to a TrainingSelfieDto object
            {
                Id = trainingSelfie.Id,
                PersonName = trainingSelfie.PersonName,
                Base64Image = trainingSelfie.Base64Image
            };

            return Ok(trainingSelfieDto); // Returns the updated TrainingSelfieDto object as a response with status code 200 OK
        }


        // Deletes a training selfie by its id.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingSelfie(int id)
        {
            var trainingSelfie = await _trainingSelfieRepository.GetByIdAsync(id); // Fetches the training selfie record by ID
            if (trainingSelfie == null)
                throw new NotFoundException("Training selfie not found.");

            await _trainingSelfieRepository.DeleteAsync(id); // Deletes the training selfie record from the repository

            return NoContent(); // Returns a 204 No Content response indicating successful deletion
        }


    }
}
