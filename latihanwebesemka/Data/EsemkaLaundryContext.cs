using latihanwebesemka.Models;
using Microsoft.EntityFrameworkCore;

namespace latihanwebesemka.Data
{
    public class EsemkaLaundryContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageTransaction> PackageTransactions { get; set; }

        public EsemkaLaundryContext(DbContextOptions<EsemkaLaundryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Email);
                entity.ToTable("Users");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Services");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Packages");
            });
            modelBuilder.Entity<PackageTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("PackageTransactions");
            });
        }
    }
}
