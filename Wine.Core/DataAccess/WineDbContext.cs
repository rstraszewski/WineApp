using System.Data.Entity;
using Wine.Core.Entities;

namespace Wine.Core.DataAccess
{
    public class WineDbContext : DbContext
    {
        public DbSet<Entities.Wine> Wines { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public WineDbContext() : base("WineDatabase")
        {
            Configuration.LazyLoadingEnabled = true;
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

            modelBuilder.Entity<Review>()
                .Property(x => x.Body)
                .HasMaxLength(2048)
                .IsRequired();

            modelBuilder.Entity<Review>()
                .Property(x => x.UserName)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Username)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasRequired(x => x.Credentials)
                .WithMany()
                .HasForeignKey(x => x.CredentialsId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Roles)
                .WithMany();

            modelBuilder.Entity<Role>()
                .Property(x => x.Name)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<Credentials>()
                .Property(x => x.PasswordHash)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<Credentials>()
                .Property(x => x.Salt)
                .HasMaxLength(32)
                .IsRequired();

        }
    }
}
