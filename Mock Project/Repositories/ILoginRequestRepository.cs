using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public interface ILoginRequestRepository
    {
        Task<LoginRequest> AddAsync(LoginRequest loginRequest);
        Task<LoginRequest> GetByUsernameAsync(string username);
        Task<LoginRequest?> GetByIdAsync(int id);
        Task<LoginRequest?> GetAdminAsync();
        Task DeleteAsync(int id);

    }
}
