using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public interface IUserFactRepository
    {
        Task AddFactAsync(UserFact fact);
        Task<UserFact> GetFactByIdAsync(int factId);
        Task UpdateFactAsync(UserFact fact);
        Task DeleteFactAsync(int factId);
    }
}
