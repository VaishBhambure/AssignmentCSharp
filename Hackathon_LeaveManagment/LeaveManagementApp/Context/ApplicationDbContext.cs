using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Reflection.Emit;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<LeaveApproval> LeaveApprovals { get; set; }
    public DbSet<LeaveBalance> LeaveBalances { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // User <--> LeaveRequests (1:M)
        builder.Entity<LeaveRequest>()
            .HasOne(l => l.Employee)
            .WithMany(u => u.LeaveRequests)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        // LeaveRequest <--> LeaveApproval (1:1)
        builder.Entity<LeaveApproval>()
            .HasOne(a => a.LeaveRequest)
            .WithOne(l => l.Approval)
            .HasForeignKey<LeaveApproval>(a => a.LeaveRequestId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        // Manager (User) <--> LeaveApproval (1:M)
        builder.Entity<LeaveApproval>()
            .HasOne(a => a.Manager)
            .WithMany(u => u.Approvals)
            .HasForeignKey(a => a.ManagerId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        // User <--> LeaveBalance (1:1)
        builder.Entity<LeaveBalance>()
            .HasOne(lb => lb.Employee)
            .WithOne(u => u.LeaveBalance)
            .HasForeignKey<LeaveBalance>(lb => lb.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        // Seed Roles
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "2", Name = "Manager", NormalizedName = "MANAGER" },
            new IdentityRole { Id = "3", Name = "Employee", NormalizedName = "EMPLOYEE" }
        );
        builder.Ignore<LeaveManagementApp.ViewModels.RegisterViewModel>();

    }
}
