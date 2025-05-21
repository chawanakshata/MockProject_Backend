
using Microsoft.AspNetCore.Mvc;
using Mock_Project.DTOs;
using Mock_Project.Exceptions;
using Mock_Project.Models;
using Mock_Project.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Mock_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingActivitiesController : ControllerBase
    {
        private readonly ITrainingActivityRepository _trainingActivityRepository;

        public TrainingActivitiesController(ITrainingActivityRepository trainingActivityRepository)
        {
            _trainingActivityRepository = trainingActivityRepository;
        }

        // Creates a new training activity from the provided data.
        [HttpPost]
        public async Task<IActionResult> CreateTrainingActivity([FromBody] TrainingActivityRequestDto trainingActivityRequestDto)
        {
            if (trainingActivityRequestDto == null)
                throw new BadRequestException("Invalid training activity data.");

            var trainingActivity = new TrainingActivity // Creates a new TrainingActivity object
            {
                Date = trainingActivityRequestDto.Date,
                Week = trainingActivityRequestDto.Week,
                DayNumber = trainingActivityRequestDto.DayNumber,
                Activity = trainingActivityRequestDto.Activity,
            };

            await _trainingActivityRepository.AddAsync(trainingActivity);

            var trainingActivityDto = new TrainingActivityDto // Creates a DTO to return the created TrainingActivity object to return in the response body
            {
                Id = trainingActivity.Id,
                Date = trainingActivity.Date,
                Week = trainingActivity.Week,
                DayNumber = trainingActivity.DayNumber,
                Activity = trainingActivity.Activity,

            };

            return Ok(trainingActivityDto);
        }

        // Retrieves all training activities.
        [HttpGet]
        public async Task<IActionResult> GetTrainingActivities()
        {
            var activities = await _trainingActivityRepository.GetAllAsync();
            var activityDtos = activities.Select(ta => new TrainingActivityDto
            {
                Id = ta.Id,
                Date = ta.Date,
                Week = ta.Week,
                DayNumber = ta.DayNumber,
                Activity = ta.Activity,
            });

            return Ok(activityDtos);
        }

        // Retrieves a specific training activity by its ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainingActivityById(int id)
        {
            var trainingActivity = await _trainingActivityRepository.GetByIdAsync(id); // Retrieves the training activity record by ID
            if (trainingActivity == null)
                throw new NotFoundException("Training Activities not found."); // Returns a 404 Not Found response if the record is not found

            var trainingActivityDto = new TrainingActivityDto // Creates a DTO to return the training activity data in the response body
            {
                Id = trainingActivity.Id,
                Date = trainingActivity.Date,
                Week = trainingActivity.Week,
                DayNumber = trainingActivity.DayNumber,
                Activity = trainingActivity.Activity
            };

            return Ok(trainingActivityDto); // Returns a 200 OK response with the training activity data
        }


        // Updates an existing training activity by its ID with new data.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingActivity(int id, [FromBody] TrainingActivityRequestDto trainingActivityRequestDto)
        {
            if (trainingActivityRequestDto == null)  // Checks if the request body is null
                throw new BadRequestException("Invalid training activity data."); // Returns a 400 Bad Request response 

            var trainingActivity = await _trainingActivityRepository.GetByIdAsync(id);  // Retrieves the training activity record by ID
            if (trainingActivity == null)
                throw new NotFoundException("Training Activity not found.");

            trainingActivity.Date = trainingActivityRequestDto.Date; // Updates the training activity record with new data
            trainingActivity.Week = trainingActivityRequestDto.Week;
            trainingActivity.DayNumber = trainingActivityRequestDto.DayNumber;
            trainingActivity.Activity = trainingActivityRequestDto.Activity;

            await _trainingActivityRepository.UpdateAsync(trainingActivity); // Updates the training activity record in the database
            return NoContent(); // Returns a 204 No Content response indicating successful update
        }

        // Deletes a specific training activity by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingActivity(int id)
        {
            var trainingActivity = await _trainingActivityRepository.GetByIdAsync(id); // Retrieves the training activity record by ID
            if (trainingActivity == null)
                throw new NotFoundException("Training Activity not found.");

            await _trainingActivityRepository.DeleteAsync(trainingActivity);
            return NoContent(); // Returns a 204 No Content response indicating successful deletion
        }
    }
}
