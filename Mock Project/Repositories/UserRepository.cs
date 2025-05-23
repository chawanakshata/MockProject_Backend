using Microsoft.EntityFrameworkCore;
using Mock_Project.Data;
using Mock_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mock_Project.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetSingleUserAsync()
        {
            return await _context.Users
                .Include(u => u.Gallery) // Eagerly loads the related Gallery collection for each user.
                .Include(u => u.Facts)
                .OrderBy(u => u.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Gallery)
                .Include(u => u.Facts)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Gallery)
                .Include(u => u.Facts)
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

    }
}
