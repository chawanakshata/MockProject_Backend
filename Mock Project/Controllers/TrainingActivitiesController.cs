
using Microsoft.AspNetCore.Mvc;
using Mock_Project.DTOs;
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

        [HttpPost]
        public async Task<IActionResult> CreateTrainingActivity([FromBody] TrainingActivityRequestDto trainingActivityRequestDto)
        {
            if (trainingActivityRequestDto == null)
                return BadRequest("Invalid training activity data.");

            var trainingActivity = new TrainingActivity
            {
                Date = trainingActivityRequestDto.Date,
                Week = trainingActivityRequestDto.Week,
                DayNumber = trainingActivityRequestDto.DayNumber,
                Activity = trainingActivityRequestDto.Activity,
            };

            await _trainingActivityRepository.AddAsync(trainingActivity);

            var trainingActivityDto = new TrainingActivityDto
            {
                Id = trainingActivity.Id,
                Date = trainingActivity.Date,
                Week = trainingActivity.Week,
                DayNumber = trainingActivity.DayNumber,
                Activity = trainingActivity.Activity,

            };

            return Ok(trainingActivityDto);
        }

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainingActivityById(int id)
        {
            var trainingActivity = await _trainingActivityRepository.GetByIdAsync(id);
            if (trainingActivity == null)
                return NotFound();

            var trainingActivityDto = new TrainingActivityDto
            {
                Id = trainingActivity.Id,
                Date = trainingActivity.Date,
                Week = trainingActivity.Week,
                DayNumber = trainingActivity.DayNumber,
                Activity = trainingActivity.Activity
            };

            return Ok(trainingActivityDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingActivity(int id, [FromBody] TrainingActivityRequestDto trainingActivityRequestDto)
        {
            if (trainingActivityRequestDto == null)
                return BadRequest("Invalid training activity data.");

            var trainingActivity = await _trainingActivityRepository.GetByIdAsync(id);
            if (trainingActivity == null)
                return NotFound();

            trainingActivity.Date = trainingActivityRequestDto.Date;
            trainingActivity.Week = trainingActivityRequestDto.Week;
            trainingActivity.DayNumber = trainingActivityRequestDto.DayNumber;
            trainingActivity.Activity = trainingActivityRequestDto.Activity;


            await _trainingActivityRepository.UpdateAsync(trainingActivity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingActivity(int id)
        {
            var trainingActivity = await _trainingActivityRepository.GetByIdAsync(id);
            if (trainingActivity == null)
                return NotFound();

            await _trainingActivityRepository.DeleteAsync(trainingActivity);

            return NoContent();
        }
    }
}
