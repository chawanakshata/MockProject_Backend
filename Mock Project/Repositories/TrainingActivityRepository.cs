using Microsoft.EntityFrameworkCore;
using Mock_Project.Data;
using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public class TrainingActivityRepository : ITrainingActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainingActivityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrainingActivity>> GetAllAsync()
        {
            return await _context.TrainingActivities.ToListAsync();
        }

        public async Task<TrainingActivity> GetByIdAsync(int id)
        {
            return await _context.TrainingActivities.FindAsync(id);
        }

        public async Task AddAsync(TrainingActivity trainingActivity)
        {
            await _context.TrainingActivities.AddAsync(trainingActivity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrainingActivity trainingActivity)
        {
            _context.TrainingActivities.Update(trainingActivity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TrainingActivity trainingActivity)
        {
            _context.TrainingActivities.Remove(trainingActivity);
            await _context.SaveChangesAsync();
        }

        
    }
}
