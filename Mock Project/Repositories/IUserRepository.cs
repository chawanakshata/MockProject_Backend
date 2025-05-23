using System.Collections.Generic;
using System.Threading.Tasks;
using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetSingleUserAsync();
        Task<User> GetByIdAsync(int id); 
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
