﻿using LeaveManagement.Models;

namespace LeaveManagement.Service
{
    public interface ILeaveApprovalService
    {
        Task<IEnumerable<LeaveApproval>> GetAllLeaveApprovalsAsync();
        Task<LeaveApproval> GetLeaveApprovalByIdAsync(int approvalId);
        Task AddLeaveApprovalAsync(LeaveApproval approval);
        Task<IEnumerable<LeaveApproval>> GetApprovalsByManagerIdAsync(int managerId);
        Task<bool> ApproveLeaveRequestAsync(int leaveRequestId, int managerId, LeaveApproval.ApprovalStatusEnum status, string managerComment);
    }
}
