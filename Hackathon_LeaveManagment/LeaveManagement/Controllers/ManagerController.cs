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

//        public async Task<IActionResult> ManagerDashboard()
//        {
//            var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync() ?? new List<LeaveRequest>();
//            return View(leaveRequests);
//        }

//        public async Task<IActionResult> ViewLeaveRequest(int id)
//        {
//            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
//            if (leaveRequest == null)
//                return NotFound();

//            return View(leaveRequest);
//        }

//        [HttpPost]
//        public async Task<IActionResult> ApproveLeave(int leaveRequestId)
//        {
//            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(leaveRequestId);
//            if (leaveRequest == null) return NotFound();

//            if (leaveRequest.Status != LeaveRequest.LeaveStatusEnum.Pending)
//            {
//                TempData["ErrorMessage"] = "Leave request has already been processed.";
//                return RedirectToAction("ManagerDashboard");
//            }

//            // ✅ Approve Leave Request
//            leaveRequest.Status = LeaveRequest.LeaveStatusEnum.Approved;
//            await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);

//            // ✅ Deduct Leave Balance
//            var leaveBalance = await _leaveBalanceService.GetLeaveBalanceByUserIdAsync(leaveRequest.UserId);
//            int leaveDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;

//            if (leaveBalance.RemainingLeaveDays >= leaveDays)
//            {
//                leaveBalance.RemainingLeaveDays -= leaveDays;
//                await _leaveBalanceService.UpdateLeaveBalanceAsync(leaveBalance);
//            }
//            else
//            {
//                TempData["ErrorMessage"] = "Insufficient leave balance.";
//                return RedirectToAction("ManagerDashboard");
//            }

//            TempData["SuccessMessage"] = "Leave request approved successfully.";
//            return RedirectToAction("ManagerDashboard");
//        }

//        [HttpPost]
//        public async Task<IActionResult> RejectLeave(int leaveRequestId)
//        {
//            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(leaveRequestId);
//            if (leaveRequest == null) return NotFound();

//            if (leaveRequest.Status != LeaveRequest.LeaveStatusEnum.Pending)
//            {
//                TempData["ErrorMessage"] = "Leave request has already been processed.";
//                return RedirectToAction("ManagerDashboard");
//            }

//            leaveRequest.Status = LeaveRequest.LeaveStatusEnum.Rejected;
//            await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);

//            TempData["SuccessMessage"] = "Leave request rejected successfully.";
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

//       [HttpPost]
//public async Task<IActionResult> ApproveLeave(int leaveRequestId, string managerComment)
//{
//    try
//    {
//        var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(leaveRequestId);
//        if (leaveRequest == null) return NotFound();

//        if (leaveRequest.Status != LeaveRequest.LeaveStatusEnum.Pending)
//        {
//            TempData["ErrorMessage"] = "Leave request has already been processed.";
//            return RedirectToAction("ManagerDashboard");
//        }

//        // ✅ Approve the leave request
//        leaveRequest.Status = LeaveRequest.LeaveStatusEnum.Approved;
//        leaveRequest.ManagerComment = string.IsNullOrEmpty(managerComment) ? "No Comment" : managerComment;
//        leaveRequest.ReviewedDate = DateTime.Now;

//        await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);

//        TempData["SuccessMessage"] = "Leave request approved successfully.";
//    }
//    catch (Exception ex)
//    {
//        TempData["ErrorMessage"] = ex.Message;
//    }

//    return RedirectToAction("ManagerDashboard");
//}


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

using System.Threading.Tasks;
using LeaveManagement.Service;

namespace LeaveManagement.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILeaveApprovalService _leaveApprovalService;
        private readonly ILeaveBalanceService _leaveBalanceService;

        public ManagerController(ILeaveRequestService leaveRequestService,
                                 ILeaveApprovalService leaveApprovalService,
                                 ILeaveBalanceService leaveBalanceService)
        {
            _leaveRequestService = leaveRequestService;
            _leaveApprovalService = leaveApprovalService;
            _leaveBalanceService = leaveBalanceService;
        }

        /// <summary>
        /// Retrieves all leave requests and their approval details.
        /// </summary>
        /// <returns>A view displaying leave requests with their approval status.</returns>
        public async Task<IActionResult> ManagerDashboard()
        {
            var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync();
            var leaveApprovals = await _leaveApprovalService.GetAllLeaveApprovalsAsync(); // Fetch all approvals

            var leaveRequestList = leaveRequests.Select(leave => new LeaveRequest
            {
                LeaveRequestId = leave.LeaveRequestId,
                UserId = leave.UserId,
                Employee = leave.Employee,
                LeaveType = leave.LeaveType,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Status = leave.Status,
                Reason = leave.Reason,
                AppliedDate = leave.AppliedDate,
                LeaveApproval = leaveApprovals.FirstOrDefault(a => a.LeaveRequestId == leave.LeaveRequestId)
            }).ToList();

            return View(leaveRequestList);
        }

        /// <summary>
        /// Retrieves the details of a specific leave request.
        /// </summary>
        /// <param name="id">The unique ID of the leave request.</param>
        /// <returns>The leave request details view.</returns>
        public async Task<IActionResult> ViewLeaveRequest(int id)
        {
            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
            if (leaveRequest == null)
                return NotFound();

            return View(leaveRequest);
        }

        /// <summary>
        /// Approves a leave request and updates the database.
        /// </summary>
        /// <param name="leaveRequestId">The ID of the leave request to approve.</param>
        /// <param name="managerComment">Optional comment from the manager.</param>
        /// <returns>Redirects to Manager Dashboard.</returns>
        [HttpPost]
        public async Task<IActionResult> ApproveLeave(int leaveRequestId, string managerComment)
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

                // ✅ Approve the leave request
                leaveRequest.Status = LeaveRequest.LeaveStatusEnum.Approved;
                await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);
                int managerId = int.Parse(User.Identity.Name);

                // ✅ Create LeaveApproval entry
                var leaveApproval = new LeaveApproval
                {
                    LeaveRequestId = leaveRequest.LeaveRequestId,
                    ManagerId = managerId, // Ensure this is set
                    ApprovalStatus = LeaveApproval.ApprovalStatusEnum.Approved,
                    ReviewedDate = DateTime.Now,
                    Comments = managerComment
                };

                await _leaveApprovalService.AddLeaveApprovalAsync(leaveApproval);

                TempData["SuccessMessage"] = "Leave request approved successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("ManagerDashboard");
        }


        /// <summary>
        /// Rejects a leave request and updates the status.
        /// </summary>
        /// <param name="leaveRequestId">The ID of the leave request to reject.</param>
        /// <param name="managerComment">Optional comment explaining the rejection.</param>
        /// <returns>Redirects to Manager Dashboard.</returns>
        [HttpPost]
        public async Task<IActionResult> RejectLeave(int leaveRequestId, string managerComment)
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

                // ✅ Save manager's comment if provided
                if (!string.IsNullOrEmpty(managerComment))
                {
                    leaveRequest.ManagerComment = managerComment;
                }

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


