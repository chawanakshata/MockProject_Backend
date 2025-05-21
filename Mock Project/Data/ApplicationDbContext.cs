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
        public DbSet<LoginRequest> LoginRequestTable { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Gallery) // User has One-to-many relationship with UserImage
                .WithOne(ui => ui.User)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Facts) // User has One-to-many relationship with UserFact
                .WithOne(uf => uf.User) 
                .HasForeignKey(uf => uf.UserId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.LoginRequest)
                .WithOne(lr => lr.User)
                .HasForeignKey<LoginRequest>(lr => lr.Id)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder); 

        }

    }
}
