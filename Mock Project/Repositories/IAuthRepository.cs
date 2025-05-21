using Mock_Project.DTOs;
using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public interface IAuthRepository
    {
        Task<User> LoginAsync(string username, string password);
        Task <User> RegisterAsync(string username, string password);

    }

}
