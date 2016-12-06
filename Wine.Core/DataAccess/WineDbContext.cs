using System.Data.Entity;

namespace Wine.Core.DataAccess
{
    public class WineDbContext : DbContext
    {
        public DbSet<Entities.Wine> Wines { get; set; }
        
        public WineDbContext() :base ("WineDatabase")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Wine>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<Entities.Wine>()
                .Property(x => x.Category)
                .IsRequired();

            modelBuilder.Entity<Entities.Wine>()
                .Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(2048);

            modelBuilder.Entity<Entities.Wine>()
                .Property(x => x.Region)
                .IsRequired()
                .HasMaxLength(128);

            modelBuilder.Entity<Entities.Wine>()
                .Property(x => x.Varietal)
                .IsRequired();

            modelBuilder.Entity<Entities.Wine>()
                .Property(x => x.Vintage)
                .IsRequired();

            modelBuilder.Entity<Entities.Wine>()
                .Property(x => x.Thumbnail)
                .IsMaxLength()
                .IsRequired();

            modelBuilder.Entity<Entities.Wine>()
                .HasKey(x => x.Id)
                .HasMany(x => x.Reviews)
                .WithRequired()
                .HasForeignKey(x => x.WineId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Entities.Review>()
                .Property(x => x.Body)
                .HasMaxLength(2048)
                .IsRequired();

            modelBuilder.Entity<Entities.Review>()
                .Property(x => x.UserName)
                .HasMaxLength(128)
                .IsRequired();
        }
    }
}
