using LeaveManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveApproval> LeaveApprovals { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var passwordHasher = new PasswordHasher<User>();
            string hashedPassword = passwordHasher.HashPassword(null, "Vaish@123");
            // Prevent Cascade Delete for LeaveApproval → User (Manager)
            modelBuilder.Entity<LeaveApproval>()
                .HasOne(la => la.Manager)
                .WithMany(u => u.Approvals)
                .HasForeignKey(la => la.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete

            // Prevent Cascade Delete for LeaveRequest → User (Employee)
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)
                .WithMany(u => u.LeaveRequests)
                .HasForeignKey(lr => lr.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete

            //  Allow Cascade Delete for LeaveApproval → LeaveRequest
            modelBuilder.Entity<LeaveApproval>()
                .HasOne(la => la.LeaveRequest) // Fixed: Changed `LeaveRequests` → `LeaveRequest`
                .WithOne(lr => lr.LeaveApproval)
                .HasForeignKey<LeaveApproval>(la => la.LeaveRequestId)
                .OnDelete(DeleteBehavior.Cascade); // Only allow cascade for this relationship

            // Adding an Admin User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Name = "Vaishnavi",
                    Email = "Vaish@gmail.com",
                    Password = hashedPassword, // ✅ Store only the hashed password
                    Role = User.Roles.Admin
                }
            );
        }
    }
}
