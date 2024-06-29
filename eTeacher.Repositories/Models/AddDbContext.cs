using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public partial class AddDbContext : IdentityDbContext<User>
	{
        
        public AddDbContext(DbContextOptions<AddDbContext> options) : base(options) { }

        public AddDbContext() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ClassHour> ClassHours { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(e => e.Requirements)
                      .WithOne()
                      .HasForeignKey(e => e.User_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.ClassHours)
                      .WithOne()
                      .HasForeignKey(e => e.User_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.TutorClasses)
                      .WithOne(c => c.Tutor)
                      .HasForeignKey(c => c.Tutor_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.StudentClasses)
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

                entity.HasMany(e => e.TutorReports)
                      .WithOne(c => c.Tutor)
                      .HasForeignKey(c => c.Tutor_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.StudentReports)
                      .WithOne(c => c.Student)
                      .HasForeignKey(c => c.Student_id)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Requirement entity configuration
            modelBuilder.Entity<Requirement>(entity =>
            {
                entity.HasKey(e => e.Requirement_id);
                entity.HasOne(e => e.Subject)
                      .WithMany(s => s.Requirements)
                      .HasForeignKey(e => e.Subject_name)
                      .HasPrincipalKey(s => s.Subject_name)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ClassHour entity configuration
            modelBuilder.Entity<ClassHour>(entity =>
            {
                entity.HasKey(e => e.Class_id);
                entity.HasOne(e => e.Subject)
                      .WithMany(s => s.ClassHours)
                      .HasForeignKey(e => e.Subject_name)
                      .HasPrincipalKey(s => s.Subject_name)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Class entity configuration
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.Class_id);
                entity.HasOne(e => e.Subject)
                      .WithMany(s => s.Classes)
                      .HasForeignKey(e => e.Subject_name)
                      .HasPrincipalKey(s => s.Subject_name)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Reports)
                      .WithOne(r => r.Class)
                      .HasForeignKey(r => r.Class_id)
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
