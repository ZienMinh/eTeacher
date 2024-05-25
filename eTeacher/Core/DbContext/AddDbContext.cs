using eTeacher.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eTeacher.Core.DbContext
{
    public class AddDbContext : IdentityDbContext<User>
    {
        public AddDbContext(DbContextOptions<AddDbContext> options) : base(options) { }

        public override DbSet<User> Users { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<Requirement> Requirements { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<Qualification> Qualifications { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Class> Classes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Users entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(e => e.Requirements)
                      .WithOne()
                      .HasForeignKey(e => e.User_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Classes)
                      .WithOne()
                      .HasForeignKey(e => e.Tutor_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Orders)
                      .WithOne()
                      .HasForeignKey(e => e.User_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Qualifications)
                      .WithOne()
                      .HasForeignKey(e => e.User_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Reports)
                      .WithOne()
                      .HasForeignKey(e => e.User_id)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //Requirement entity configuration
            modelBuilder.Entity<Requirement>(entity =>
            {
                entity.HasKey(e => e.Requirement_id);
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.User_id)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //Wallet entity configuration
            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(e => e.Wallet_id);
                entity.HasOne<User>()
                      .WithOne()
                      .HasForeignKey<User>(e => e.Wallet_id)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //Classes entity configuration
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.Class_id);

                entity.HasOne<User>()
                        .WithMany()
                        .HasForeignKey(e => e.Student_id)
                        .HasPrincipalKey(u => u.Id)
                        .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                        .WithMany()
                        .HasForeignKey(e => e.Tutor_id)
                        .HasPrincipalKey(u => u.Id)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            //Order entity configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Order_id);
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.User_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Wallet>()
                      .WithMany()
                      .HasForeignKey(e => e.Wallet_id)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //Qualification entity configuration
            modelBuilder.Entity<Qualification>(entity =>
            {
                entity.HasKey(e => e.Qualification_id);
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.User_id)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //Report entity configuration
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.Report_id);
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.User_id)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

