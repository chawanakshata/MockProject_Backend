using Microsoft.EntityFrameworkCore;
using Mock_Project.Data;
using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public class UserFactRepository : IUserFactRepository
    {
        private readonly ApplicationDbContext _context;

        public UserFactRepository(ApplicationDbContext context)
        {
            _context = context;
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
