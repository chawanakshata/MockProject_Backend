using Microsoft.EntityFrameworkCore;
using Mock_Project.Data;
using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public class TrainingSelfieRepository : ITrainingSelfieRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainingSelfieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrainingSelfie>> GetAllAsync()
        {
            return await _context.TrainingSelfies.ToListAsync();
        }

        public async Task<TrainingSelfie> GetByIdAsync(int id)
        {
            return await _context.TrainingSelfies.FindAsync(id);
        }

        public async Task AddAsync(TrainingSelfie trainingSelfie)
        {
            await _context.TrainingSelfies.AddAsync(trainingSelfie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrainingSelfie trainingSelfie)
        {
            _context.TrainingSelfies.Update(trainingSelfie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) 
        {
            var trainingSelfie = await _context.TrainingSelfies.FindAsync(id);
            if (trainingSelfie != null)
            {
                _context.TrainingSelfies.Remove(trainingSelfie);
                await _context.SaveChangesAsync();
            }
        }
    }
}
