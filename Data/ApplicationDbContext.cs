using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InventoryApp.Models;

namespace InventoryApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AspNetUserWorkStation> AspNetUserWorkStations { get; set; }
        public DbSet<WorkStation> WorkStations { get; set; }
        public DbSet<DeviceModel> DeviceModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AspNetUserWorkStation>()
                .HasKey(uw => new { uw.UserId, uw.WorkStationId });

            modelBuilder.Entity<AspNetUserWorkStation>()
                .HasOne(uw => uw.User)
                .WithMany()
                .HasForeignKey(uw => uw.UserId);

            modelBuilder.Entity<AspNetUserWorkStation>()
                .HasOne(uw => uw.WorkStation)
                .WithOne(ws => ws.UserWorkStation)
                .HasForeignKey<AspNetUserWorkStation>(uw => uw.WorkStationId);

            modelBuilder.Entity<WorkStation>()
                .HasIndex(ws => ws.WorkStationNumber)
                .IsUnique();
        }
    }
}
