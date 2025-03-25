//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using LeaveManagement.Services;
//using LeaveManagement.Models;
//using System.Threading.Tasks;
//using LeaveManagement.Service;

//namespace LeaveManagement.Controllers
//{
//    [Authorize(Roles = "Manager")]
//    public class ManagerController : Controller
//    {
//        private readonly ILeaveRequestService _leaveRequestService;
//        private readonly ILeaveApprovalService _leaveApprovalService;
//        private readonly ILeaveBalanceService _leaveBalanceService;

//        public ManagerController(ILeaveRequestService leaveRequestService,
//                                 ILeaveApprovalService leaveApprovalService,
//                                 ILeaveBalanceService leaveBalanceService)
//        {
//            _leaveRequestService = leaveRequestService;
//            _leaveApprovalService = leaveApprovalService;
//            _leaveBalanceService = leaveBalanceService;
//        }

//        // ✅ Manager Dashboard - Shows Pending, Approved & Rejected Requests
//        public async Task<IActionResult> ManagerDashboard()
//        {
//            var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync();
//            var leaveApprovals = await _leaveApprovalService.GetAllLeaveApprovalsAsync(); // Get all approvals

//            var leaveRequestList = leaveRequests.Select(leave => new LeaveRequest
//            {
//                LeaveRequestId = leave.LeaveRequestId,
//                UserId = leave.UserId,
//                Employee = leave.Employee, // Navigation property
//                LeaveType = leave.LeaveType,
//                StartDate = leave.StartDate,
//                EndDate = leave.EndDate,
//                Status = leave.Status,
//                Reason = leave.Reason, // Employee's reason for leave
//                AppliedDate = leave.AppliedDate,
//                LeaveApproval = leaveApprovals.FirstOrDefault(a => a.LeaveRequestId == leave.LeaveRequestId) // Fetch approval details
//            }).ToList();

//            return View(leaveRequestList);
//        }

//        // ✅ View Individual Leave Request Details
//        public async Task<IActionResult> ViewLeaveRequest(int id)
//        {
//            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
//            if (leaveRequest == null)
//                return NotFound();

//            return View(leaveRequest);
//        }

//        [HttpPost]
//        public async Task<IActionResult> ApproveLeave(int leaveRequestId, string managerComment)
//        {
//            try
//            {
//                var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(leaveRequestId);
//                if (leaveRequest == null) return NotFound();

//                if (leaveRequest.Status != LeaveRequest.LeaveStatusEnum.Pending)
//                {
//                    TempData["ErrorMessage"] = "Leave request has already been processed.";
//                    return RedirectToAction("ManagerDashboard");
//                }

//                // ✅ Approve the leave request
//                leaveRequest.Status = LeaveRequest.LeaveStatusEnum.Approved;
//                leaveRequest.ManagerComment = string.IsNullOrEmpty(managerComment) ? "No Comment" : managerComment;


//                await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);

//                TempData["SuccessMessage"] = "Leave request approved successfully.";
//            }
//            catch (Exception ex)
//            {
//                TempData["ErrorMessage"] = ex.Message;
//            }

//            return RedirectToAction("ManagerDashboard");
//        }


//        // ✅ Reject Leave Request
//        [HttpPost]
//        public async Task<IActionResult> RejectLeave(int leaveRequestId)
//        {
//            try
//            {
//                var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(leaveRequestId);
//                if (leaveRequest == null) return NotFound();

//                if (leaveRequest.Status != LeaveRequest.LeaveStatusEnum.Pending)
//                {
//                    TempData["ErrorMessage"] = "Leave request has already been processed.";
//                    return RedirectToAction("ManagerDashboard");
//                }

//                // ✅ Reject the leave request
//                leaveRequest.Status = LeaveRequest.LeaveStatusEnum.Rejected;
//                await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);

//                TempData["SuccessMessage"] = "Leave request rejected successfully.";
//            }
//            catch (Exception ex)
//            {
//                TempData["ErrorMessage"] = ex.Message;
//            }

//            return RedirectToAction("ManagerDashboard");
//        }
//    }
//}



using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveManagement.Services;
using LeaveManagement.Models;
using LeaveManagement.Service;
using LeaveManagement.Exceptions;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Context;

namespace LeaveManagement.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILeaveApprovalService _leaveApprovalService;
        private readonly ILeaveBalanceService _leaveBalanceService;
        private readonly ApplicationDbContext _context;

        public ManagerController(ILeaveRequestService leaveRequestService,
                                 ILeaveApprovalService leaveApprovalService,
                                 ILeaveBalanceService leaveBalanceService,
                                 ApplicationDbContext context)
        {
            _leaveRequestService = leaveRequestService;
            _leaveApprovalService = leaveApprovalService;
            _leaveBalanceService = leaveBalanceService;
            _context = context;
        }
      

        // ✅ Manager Dashboard - Shows Pending, Approved & Rejected Requests
        public async Task<IActionResult> ManagerDashboard()
        {
            var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync();
            var leaveApprovals = await _leaveApprovalService.GetAllLeaveApprovalsAsync(); // Get all approvals

            var leaveRequestList = leaveRequests.Select(leave => new LeaveRequest
            {
                LeaveRequestId = leave.LeaveRequestId,
                UserId = leave.UserId,
                Employee = leave.Employee, // Navigation property
                LeaveType = leave.LeaveType,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Status = leave.Status,
                Reason = leave.Reason, // Employee's reason for leave
                AppliedDate = leave.AppliedDate,
                LeaveApproval = leaveApprovals.FirstOrDefault(a => a.LeaveRequestId == leave.LeaveRequestId) // Fetch approval details
            }).ToList();

            return View(leaveRequestList);
        }

        // ✅ View Individual Leave Request Details
        public async Task<IActionResult> ViewLeaveRequest(int id)
        {
            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
            if (leaveRequest == null)
                return NotFound();

            return View(leaveRequest);
        }
        [HttpPost()]
        public async Task<IActionResult> ApproveLeave(int leaveRequestId, string managerComment)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
            if (leaveRequest == null)
                return NotFound();

            // Fetch Leave Balance
            var leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(lb => lb.UserId == leaveRequest.UserId);
            if (leaveBalance == null)
                return BadRequest("Leave balance record not found.");

            // ✅ Check if there's enough leave balance
            if (leaveBalance.RemainingLeaveDays < leaveRequest.NumberOfDays)
                return BadRequest("Insufficient leave balance.");

            // ✅ Deduct leave balance
            leaveBalance.RemainingLeaveDays -= leaveRequest.NumberOfDays;
            leaveBalance.LastUpdate = DateTime.UtcNow;

            // ✅ Update Leave Request Status to Approved
            leaveRequest.Status = LeaveRequest.LeaveStatusEnum.Approved;
            leaveRequest.ManagerComment = managerComment;

            _context.LeaveBalances.Update(leaveBalance);
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Leave request approved successfully.";
            return RedirectToAction("ManagerDashboard");
        }



        // ✅ Reject Leave Request
        [HttpPost]
        public async Task<IActionResult> RejectLeave(int leaveRequestId)
        {
            try
            {
                var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(leaveRequestId);
                if (leaveRequest == null) return NotFound();

                if (leaveRequest.Status != LeaveRequest.LeaveStatusEnum.Pending)
                {
                    TempData["ErrorMessage"] = "Leave request has already been processed.";
                    return RedirectToAction("ManagerDashboard");
                }

                // ✅ Reject the leave request
                leaveRequest.Status = LeaveRequest.LeaveStatusEnum.Rejected;
                await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);

                TempData["SuccessMessage"] = "Leave request rejected successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("ManagerDashboard");
        }
    }
}


