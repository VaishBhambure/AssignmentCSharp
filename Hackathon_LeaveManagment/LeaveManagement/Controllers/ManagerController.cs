using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveManagement.Models;
using LeaveManagement.Service;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LeaveManagement.Services;

namespace LeaveManagement.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILeaveApprovalService _leaveApprovalService;

        public ManagerController(ILeaveRequestService leaveRequestService,
                                 ILeaveApprovalService leaveApprovalService)
        {
            _leaveRequestService = leaveRequestService;
            _leaveApprovalService = leaveApprovalService;
        }

        // Displays the dashboard with all leave requests.
        public async Task<IActionResult> ManagerDashboard()
        {
            var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync()
                                ?? new List<LeaveRequest>();
            return View(leaveRequests);
        }

        // Displays a specific leave request.
        public async Task<IActionResult> ViewLeaveRequest(int id)
        {
            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
            if (leaveRequest == null)
                return NotFound();

            return View(leaveRequest);
        }

        // POST: Approve a leave request using the leave approval service.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveLeave(int leaveRequestId, string managerComment)
        {
            int managerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                bool result = await _leaveApprovalService.ApproveLeaveRequestAsync(
                    leaveRequestId,
                    managerId,
                    isApproved: true,
                    managerComment: managerComment
                );

                TempData["SuccessMessage"] = result
                    ? "Leave request approved successfully."
                    : "Error processing leave request.";
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("ManagerDashboard");
        }

        // POST: Reject a leave request using the leave approval service.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectLeave(int leaveRequestId, string managerComment)
        {
            int managerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                bool result = await _leaveApprovalService.ApproveLeaveRequestAsync(
                    leaveRequestId,
                    managerId,
                    isApproved: false,
                    managerComment: managerComment
                );

                TempData["SuccessMessage"] = result
                    ? "Leave request rejected successfully."
                    : "Error processing leave request.";
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("ManagerDashboard");
        }
    }
}



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



//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using LeaveManagement.Services;
//using LeaveManagement.Models;

//using System.Threading.Tasks;
//using LeaveManagement.Service;
//using Microsoft.EntityFrameworkCore;

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

//        /// <summary>
//        /// Retrieves all leave requests and their approval details.
//        /// </summary>
//        /// <returns>A view displaying leave requests with their approval status.</returns>
//        public async Task<IActionResult> ManagerDashboard()
//        {
//            var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync();
//            var leaveApprovals = await _leaveApprovalService.GetAllLeaveApprovalsAsync(); // Fetch all approvals

//            var leaveRequestList = leaveRequests.Select(leave => new LeaveRequest
//            {
//                LeaveRequestId = leave.LeaveRequestId,
//                UserId = leave.UserId,
//                Employee = leave.Employee,
//                LeaveType = leave.LeaveType,
//                StartDate = leave.StartDate,
//                EndDate = leave.EndDate,
//                Status = leave.Status,
//                Reason = leave.Reason,
//                AppliedDate = leave.AppliedDate,
//                LeaveApproval = leaveApprovals.FirstOrDefault(a => a.LeaveRequestId == leave.LeaveRequestId)
//            }).ToList();

//            return View(leaveRequestList);
//        }

//        /// <summary>
//        /// Retrieves the details of a specific leave request.
//        /// </summary>
//        /// <param name="id">The unique ID of the leave request.</param>
//        /// <returns>The leave request details view.</returns>
//        public async Task<IActionResult> ViewLeaveRequest(int id)
//        {
//            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
//            if (leaveRequest == null)
//                return NotFound();

//            return View(leaveRequest);
//        }

//        /// <summary>
//        /// Approves a leave request and updates the database.
//        /// </summary>
//        /// <param name="leaveRequestId">The ID of the leave request to approve.</param>
//        /// <param name="managerComment">Optional comment from the manager.</param>
//        /// <returns>Redirects to Manager Dashboard.</returns>
//        [HttpPost]
//        public IActionResult ApproveLeave(int id)
//        {
//            var leaveRequest = _leaveRequestService.LeaveRequests.Find(id);
//            if (leaveRequest == null)
//            {
//                return NotFound();
//            }

//            leaveRequest.Status = "Approved";
//            _leaveRequestService.SaveChanges();

//            return RedirectToAction("ManagerDashboard");
//        }



//        /// <summary>
//        /// Rejects a leave request and updates the status.
//        /// </summary>
//        /// <param name="leaveRequestId">The ID of the leave request to reject.</param>
//        /// <param name="managerComment">Optional comment explaining the rejection.</param>
//        /// <returns>Redirects to Manager Dashboard.</returns>
//        [HttpPost]
//        public async Task<IActionResult> RejectLeave(int leaveRequestId, string managerComment)
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

//                // ✅ Save manager's comment if provided
//                if (!string.IsNullOrEmpty(managerComment))
//                {
//                    leaveRequest.ManagerComment = managerComment;
//                }

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


