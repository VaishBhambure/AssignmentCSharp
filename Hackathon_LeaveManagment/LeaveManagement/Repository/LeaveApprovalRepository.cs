using LeaveManagement.Context;
using LeaveManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class LeaveApprovalRepository : ILeaveApprovalRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveApprovalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveApproval>> GetAllLeaveApprovalsAsync()
        {
            return await _context.LeaveApprovals
                                 .Include(la => la.LeaveRequest)
                                 .Include(la => la.Manager)
                                 .ToListAsync();
        }

        public async Task<LeaveApproval> GetLeaveApprovalByIdAsync(int approvalId)
        {
            return await _context.LeaveApprovals
                                 .Include(la => la.LeaveRequest)
                                 .Include(la => la.Manager)
                                 .FirstOrDefaultAsync(la => la.ApprovalId == approvalId);
        }

        public async Task AddLeaveApprovalAsync(LeaveApproval approval)
        {
            await _context.LeaveApprovals.AddAsync(approval);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Log detailed error information
                Console.WriteLine("Error saving changes in AddLeaveApprovalAsync:");
                Console.WriteLine("Message: " + dbEx.Message);
                Console.WriteLine("Inner Exception: " + dbEx.InnerException?.Message);
                throw; // Optionally rethrow or handle accordingly
            }
        }

        public async Task<IEnumerable<LeaveApproval>> GetApprovalsByManagerIdAsync(int managerId)
        {
            return await _context.LeaveApprovals
                                 .Where(la => la.ManagerId == managerId)
                                 .Include(la => la.LeaveRequest)
                                 .ToListAsync();
        }

        public async Task<bool> UpdateLeaveApprovalAsync(LeaveApproval leaveApproval)
        {
            Console.WriteLine($"Updating LeaveApproval: ApprovalId={leaveApproval.ApprovalId}, Comments={leaveApproval.Comments}");

            _context.LeaveApprovals.Update(leaveApproval);
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                // Log detailed error information
                Console.WriteLine("Error saving changes in UpdateLeaveApprovalAsync:");
                Console.WriteLine("Message: " + dbEx.Message);
                Console.WriteLine("Inner Exception: " + dbEx.InnerException?.Message);
                throw; // Optionally rethrow or handle accordingly
            }
        }
    }
}
