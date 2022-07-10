using Microsoft.EntityFrameworkCore;
using ThePenaltyBox.Models;

namespace ThePenaltyBox.Data
{
    public class PenaltyContext : DbContext
    {

        public PenaltyContext(DbContextOptions<PenaltyContext> options) : base(options)
        {}

        public DbSet<Penalty> Penalties { get; set; }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Penalty>()
                        .Property(e => e.Referees)
                        .HasConversion(
                            v => string.Join(',', v),
                            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
        #endregion
    }
}
