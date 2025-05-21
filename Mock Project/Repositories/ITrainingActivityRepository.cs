using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public interface ITrainingActivityRepository
    {
        Task<IEnumerable<TrainingActivity>> GetAllAsync();
        Task<TrainingActivity> GetByIdAsync(int id);
        Task AddAsync(TrainingActivity trainingActivity);
        Task UpdateAsync(TrainingActivity trainingActivity);
        Task DeleteAsync(TrainingActivity trainingActivity);

    }
}
