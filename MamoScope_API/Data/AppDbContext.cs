using MaMoScope_API.Data;
using Microsoft.EntityFrameworkCore;

namespace MamoScope_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<MotorDrivers> MotorDrivers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MotorDrivers>(entity =>
            {
                entity.ToTable("MotorDrivers", "public");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Voltage)
                    .IsRequired();

                entity.Property(e => e.TestDate)
                    .IsRequired();

                entity.Property(e => e.IsPassed)
                    .IsRequired();
            });
        }
    }
}