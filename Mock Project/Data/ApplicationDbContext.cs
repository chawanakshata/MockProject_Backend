using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Mock_Project.Models;

namespace Mock_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<TeamSelfie> TeamSelfies { get; set; }
        public DbSet<TrainingSelfie> TrainingSelfies { get; set; }
        public DbSet<TrainingActivity> TrainingActivities { get; set; }
        public DbSet<UserFact> UserFacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<User>()
                .HasMany(u => u.Gallery)
                .WithOne(ui => ui.User)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Facts) 
                .WithOne(uf => uf.User) 
                .HasForeignKey(uf => uf.UserId) 
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

        }

    }
}
