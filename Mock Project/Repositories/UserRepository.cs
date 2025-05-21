
//using Microsoft.EntityFrameworkCore;
//using Mock_Project.Data;
//using Mock_Project.Models;

//namespace Mock_Project.Repositories
//{
//    public class UserRepository : IUserRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public UserRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<User> GetSingleUserAsync()
//        {
//            // Order by Id to ensure consistent results
//            return await _context.Users
//                .Include(u => u.Gallery)
//                .Include(u => u.Facts)
//                .OrderBy(u => u.Id) // Add OrderBy clause
//                .FirstOrDefaultAsync();
//        }

//        public async Task<User> GetByIdAsync(int id)
//        {
//            return await _context.Users
//                .Include(u => u.Gallery)
//                .Include(u => u.Facts)
//                .FirstOrDefaultAsync(u => u.Id == id);
//        }

//        public async Task AddAsync(User user)
//        {
//            await _context.Users.AddAsync(user);
//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateAsync(User user)
//        {
//            _context.Users.Update(user);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var user = await _context.Users.FindAsync(id);
//            if (user != null)
//            {
//                _context.Users.Remove(user);
//                await _context.SaveChangesAsync();
//            }
//        }

//        public async Task AddFactAsync(UserFact fact)
//        {
//            await _context.UserFacts.AddAsync(fact);
//            await _context.SaveChangesAsync();
//        }

//        public async Task<UserFact> GetFactByIdAsync(int factId)
//        {
//            return await _context.UserFacts.FindAsync(factId);
//        }

//        public async Task UpdateFactAsync(UserFact fact)
//        {
//            _context.UserFacts.Update(fact);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteFactAsync(int factId)
//        {
//            var fact = await _context.UserFacts.FindAsync(factId);
//            if (fact != null)
//            {
//                _context.UserFacts.Remove(fact);
//                await _context.SaveChangesAsync();
//            }
//        }
//    }
//}

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

        public async Task AddFactAsync(UserFact fact)
        {
            await _context.UserFacts.AddAsync(fact);
            await _context.SaveChangesAsync();
        }

        public async Task<UserFact> GetFactByIdAsync(int factId)
        {
            return await _context.UserFacts.FindAsync(factId);
        }

        public async Task UpdateFactAsync(UserFact fact)
        {
            _context.UserFacts.Update(fact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFactAsync(int factId)
        {
            var fact = await _context.UserFacts.FindAsync(factId);
            if (fact != null)
            {
                _context.UserFacts.Remove(fact);
                await _context.SaveChangesAsync();
            }
        }
    }
}
