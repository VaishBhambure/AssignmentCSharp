using LeaveManagement.Models;
using LeaveManagement.Utilities;
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

            // Hash passwords using the custom PasswordHasher utility
            string adminHashedPassword = PasswordHasher.HashPassword("Vaish@123");
            string managerHashedPassword = PasswordHasher.HashPassword("Bhambure@123");

            modelBuilder.Entity<User>().HasData(
               new User
               {
                   UserId = 1,
                   Name = "Vaishnavi",
                   Email = "Vaish@gmail.com",
                   Password = adminHashedPassword, // Hashed password
                   Role = User.Roles.Admin
               },
               new User
               {
                   UserId = 2,
                   Name = "Bhambure",
                   Email = "bhambure@gmail.com",
                   Password = managerHashedPassword, // Hashed password
                   Role = User.Roles.Manager
               }
           );

          

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

            // Allow Cascade Delete for LeaveApproval → LeaveRequest
            modelBuilder.Entity<LeaveApproval>()
                .HasOne(la => la.LeaveRequest)
                .WithOne(lr => lr.LeaveApproval)
                .HasForeignKey<LeaveApproval>(la => la.LeaveRequestId)
                .OnDelete(DeleteBehavior.Cascade); // Allows cascade delete
        }
    }
}
