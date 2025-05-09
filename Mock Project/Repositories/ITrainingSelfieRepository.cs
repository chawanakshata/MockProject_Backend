using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public interface ITrainingSelfieRepository
    {
        Task<IEnumerable<TrainingSelfie>> GetAllAsync();
        Task<TrainingSelfie> GetByIdAsync(int id);
        Task AddAsync(TrainingSelfie trainingSelfie);
        Task UpdateAsync(TrainingSelfie trainingSelfie);
        Task DeleteAsync(int id);
    }
}
