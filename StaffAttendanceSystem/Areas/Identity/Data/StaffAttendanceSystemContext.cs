using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StaffAttendanceSystem.Areas.Identity.Data;

namespace StaffAttendanceSystem.Data;

public class StaffAttendanceSystemContext : IdentityDbContext<StaffAttendanceSystemUser>
{
    public StaffAttendanceSystemContext(DbContextOptions<StaffAttendanceSystemContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=DESKTOP-01QMQ7F;Database=AttendanceManagement;Trusted_Connection=True;TrustServerCertificate=True");
    }
}
