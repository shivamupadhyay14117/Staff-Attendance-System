using Microsoft.EntityFrameworkCore;
using StaffAttendanceSystem.Models;
using StaffAttendanceSystem.Models;

namespace StaffAttendanceSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Teacher> TeacherDetails { get; set; }
        public DbSet<Attendance> AttendanceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Attendance>()
                .HasKey(a => a.AttendanceId);

            // Add any additional configuration here (e.g., relationships, constraints).
        }


    }
}
