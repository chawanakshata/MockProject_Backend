using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public interface ITeamSelfieRepository
    {
        Task<IEnumerable<TeamSelfie>> GetAllAsync();
        Task<TeamSelfie> GetByIdAsync(int id);
        Task AddAsync(TeamSelfie entity);
        Task UpdateAsync(TeamSelfie entity);
        Task DeleteAsync(int id);
    }
}
