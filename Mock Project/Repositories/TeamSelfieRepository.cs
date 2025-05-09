using Microsoft.EntityFrameworkCore;
using Mock_Project.Data;
using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public class TeamSelfieRepository : ITeamSelfieRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamSelfieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeamSelfie>> GetAllAsync()
        {
            return await _context.TeamSelfies.ToListAsync();
        }

        public async Task<TeamSelfie> GetByIdAsync(int id)
        {
            return await _context.TeamSelfies.FindAsync(id);
        }

        public async Task AddAsync(TeamSelfie entity)
        {
            await _context.TeamSelfies.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TeamSelfie entity)
        {
            _context.TeamSelfies.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.TeamSelfies.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
