using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public class User : IdentityUser
{

    public enum UserRole
    {
        Admin = 1,
        Manager,
        Employee
    }
    public string Name { get; set; }
    public UserRole Role { get; set; } = UserRole.Employee; // Default Role

    // Navigation Properties
    public List<LeaveRequest> LeaveRequests { get; set; } // For Employees
    public List<LeaveApproval> Approvals { get; set; } // For Managers
    public LeaveBalance LeaveBalance { get; set; } // Each user has one LeaveBalance
}
