using Microsoft.EntityFrameworkCore;

namespace eTeacher.Data
{
    public class AddDbContext : DbContext
    {
        public AddDbContext(DbContextOptions<AddDbContext> options) : base(options) { }

        #region DbSet
        public DbSet<Users> Users { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<Requirement> Requirements { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<Qualification> Qualifications { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Classes> Classes { get; set; }
        #endregion
    }
}
