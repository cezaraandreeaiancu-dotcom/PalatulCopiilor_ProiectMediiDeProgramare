using Microsoft.EntityFrameworkCore;
using Palatul_Copiilor.Models;

namespace Palatul_Copiilor.Data
{
    public class Palatul_CopiilorContext : DbContext
    {
        public Palatul_CopiilorContext(DbContextOptions<Palatul_CopiilorContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Department { get; set; } = default!;
        public DbSet<Activity> Activity { get; set; } = default!;
        public DbSet<Teacher> Teacher { get; set; } = default!;
        public DbSet<Reservation> Reservation { get; set; } = default!;
        public DbSet<Review> Review { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Un participant (UserId) nu poate rezerva aceeași activitate (ActivityId) de două ori
            modelBuilder.Entity<Reservation>()
                .HasIndex(r => new { r.UserId, r.ActivityId })
                .IsUnique();

            // ✅ O rezervare poate avea UN SINGUR review
            modelBuilder.Entity<Review>()
                .HasIndex(r => r.ReservationId)
                .IsUnique();
        }
    }
}
