using eTeacher.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eTeacher.Core.DbContext
{
    public class AddDbContext : IdentityDbContext<User>
    {
        public AddDbContext(DbContextOptions<AddDbContext> options) : base(options) { }

        public override DbSet<User> Users { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Otp> Otps { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(e => e.Requirements)
                      .WithOne()
                      .HasForeignKey(e => e.User_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Classes)
                      .WithOne()
                      .HasForeignKey(c => c.Tutor_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Classes)
                      .WithOne(c => c.Student)
                      .HasForeignKey(c => c.Student_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Orders)
                      .WithOne()
                      .HasForeignKey(o => o.User_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Qualifications)
                      .WithOne()
                      .HasForeignKey(q => q.User_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Reports)
                      .WithOne()
                      .HasForeignKey(r => r.User_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Otps)
                      .WithOne(o => o.User)
                      .HasForeignKey(o => o.User_id)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Requirement>(entity =>
            {
                entity.HasKey(e => e.Requirement_id);
                entity.HasOne(e => e.Subject)
                      .WithMany()
                      .HasForeignKey(e => e.Subject_name)
                      .HasPrincipalKey(s => s.Subject_name)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Class entity configuration
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.Class_id);
                entity.HasOne(e => e.Subject)
                      .WithMany()
                      .HasForeignKey(e => e.Subject_name)
                      .HasPrincipalKey(s => s.Subject_name)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Order entity configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Order_id);
            });

            // Qualification entity configuration
            modelBuilder.Entity<Qualification>(entity =>
            {
                entity.HasKey(e => e.Qualification_id);
            });

            // Report entity configuration
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.Report_id);
            });

            // Otp entity configuration
            modelBuilder.Entity<Otp>(entity =>
            {
                entity.HasKey(e => e.Otp_id);
                entity.HasOne(o => o.User)
                      .WithMany(u => u.Otps)
                      .HasForeignKey(o => o.User_id)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Subject entity configuration
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.Subject_id);
                entity.HasIndex(e => e.Subject_name)
                      .IsUnique();
            });
        }
    }
}
