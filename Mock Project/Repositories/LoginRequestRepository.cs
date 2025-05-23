using Microsoft.EntityFrameworkCore;
using Mock_Project.Data;
using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public class LoginRequestRepository : ILoginRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public LoginRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LoginRequest> AddAsync(LoginRequest loginRequest)
        {
            _context.LoginRequests.Add(loginRequest);
            await _context.SaveChangesAsync();
            return loginRequest;
        }
        public async Task<LoginRequest> GetByUsernameAsync(string username)
        {
            return await _context.LoginRequests
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<LoginRequest?> GetAdminAsync()
        {
            return await _context.LoginRequests.FirstOrDefaultAsync(u => u.Role == "admin");
        }


        public async Task<LoginRequest?> GetByIdAsync(int id)
        {
            return await _context.LoginRequests.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.LoginRequests.FindAsync(id);
            if (entity != null)
            {
                _context.LoginRequests.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
