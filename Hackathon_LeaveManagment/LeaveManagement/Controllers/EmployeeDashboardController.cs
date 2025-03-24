//using LeaveManagement.Models;
//using LeaveManagement.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace LeaveManagement.Controllers
//{
//    //[Authorize] // Ensures only authenticated users can access

//    [Authorize(Roles = "Employee")]
//    public class EmployeeLeaveController : Controller
//    {
//        private readonly ILeaveRequestService _leaveRequestService;

//        public EmployeeLeaveController(ILeaveRequestService leaveRequestService)
//        {
//            _leaveRequestService = leaveRequestService;
//        }

//        // GET: EmployeeLeave/Dashboard

//        //public async Task<IActionResult> Dashboard()
//        //{
//        //    //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//        //    //if (string.IsNullOrEmpty(userIdClaim)) return RedirectToAction("Login", "Account");

//        //    //int userId = int.Parse(userIdClaim);
//        //    //var leaveRequests = await _leaveRequestService.GetLeaveRequestsByUserIdAsync(userId);

//        //    //return View(leaveRequests);
//        //    var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync();
//        //    return View(leaveRequests);
//        //}

//        public async Task<IActionResult> Dashboard()
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (string.IsNullOrEmpty(userIdClaim)) return RedirectToAction("Login", "User");

//            int userId = int.Parse(userIdClaim);
//            var leaveRequests = await _leaveRequestService.GetLeaveRequestsByUserIdAsync(userId);

//            return View(leaveRequests);
//        }
//        // GET: EmployeeLeave/Apply
//        public IActionResult Apply()
//        {
//            return View();
//        }

//        // POST: EmployeeLeave/Apply
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Apply(LeaveRequest leaveRequest)
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (string.IsNullOrEmpty(userIdClaim)) return RedirectToAction("Login", "Account");

//            int userId = int.Parse(userIdClaim);
//            leaveRequest.UserId = userId;

//            try
//            {
//                await _leaveRequestService.AddLeaveRequestAsync(leaveRequest);
//                TempData["Success"] = "Leave request submitted successfully!";
//                return RedirectToAction("Dashboard");
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = ex.Message;
//                return View(leaveRequest);
//            }
//        }
//    }
//}



using LeaveManagement.Models;
using LeaveManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using static LeaveManagement.Models.LeaveRequest;
using LeaveManagement.Service;

namespace LeaveManagement.Controllers
{
    [Authorize(Roles = "Employee")] // Restrict access to Employees only
    public class EmployeeLeaveController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILeaveBalanceService _leaveBalanceService;


        public EmployeeLeaveController(ILeaveRequestService leaveRequestService, ILeaveBalanceService leaveBalanceService)
        {
            _leaveRequestService = leaveRequestService;
            _leaveBalanceService = leaveBalanceService;
        }


        // Employee Dashboard - Shows Pending, Approved, Rejected Leave Requests
        //public async Task<IActionResult> Dashboard()
        //{
        //    var userId = GetUserIdFromClaims();
        //    if (userId == null) return RedirectToAction("Login", "User");

        //    var leaveRequests = await _leaveRequestService.GetLeaveRequestsByUserIdAsync(userId.Value);
        //    return View(leaveRequests);

        public async Task<IActionResult> Dashboard()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return RedirectToAction("Login", "User");

            var leaveRequests = await _leaveRequestService.GetLeaveRequestsByUserIdAsync(userId.Value);
            var leaveBalance = await _leaveBalanceService.GetLeaveBalanceByUserIdAsync(userId.Value);

            // Pass both leave requests and leave balance to the view
            return View(Tuple.Create(leaveRequests, leaveBalance));
        }


        // GET: EmployeeLeave/Apply
        public IActionResult Apply()
        {
            return View();
        }

        // POST: EmployeeLeave/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(LeaveRequest leaveRequest)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return RedirectToAction("Login", "User");

            leaveRequest.UserId = userId.Value;

            try
            {
                await _leaveRequestService.AddLeaveRequestAsync(leaveRequest);
                TempData["Success"] = "Leave request submitted successfully!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(leaveRequest);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteLeaveRequest(int requestId)
        {
            try
            {
                await _leaveRequestService.DeleteLeaveRequestAsync(requestId);
                TempData["SuccessMessage"] = "Leave request deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Dashboard");
        }
        // GET: EmployeeLeave/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return RedirectToAction("Login", "User");

            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);

            if (leaveRequest == null || leaveRequest.UserId != userId.Value)
            {
                return NotFound(); // Return 404 if leave request does not belong to the user
            }

            if (leaveRequest.Status != LeaveStatusEnum.Pending)
            {
                TempData["ErrorMessage"] = "You can only edit pending leave requests.";
                return RedirectToAction("Dashboard");
            }

            return View(leaveRequest);
        }
        // POST: EmployeeLeave/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveRequest updatedLeaveRequest)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return RedirectToAction("Login", "User");

            var existingRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);

            if (existingRequest == null || existingRequest.UserId != userId.Value)
            {
                return NotFound();
            }

            if (existingRequest.Status != LeaveStatusEnum.Pending)
            {
                TempData["ErrorMessage"] = "You can only update pending leave requests.";
                return RedirectToAction("Dashboard");
            }

            existingRequest.StartDate = updatedLeaveRequest.StartDate;
            existingRequest.EndDate = updatedLeaveRequest.EndDate;
            existingRequest.LeaveType = updatedLeaveRequest.LeaveType;
            existingRequest.Reason = updatedLeaveRequest.Reason;

            try
            {
                await _leaveRequestService.UpdateLeaveRequestAsync(existingRequest);
                TempData["SuccessMessage"] = "Leave request updated successfully!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(updatedLeaveRequest);
            }
        }

        [HttpPost]
        public async Task<IActionResult> LeaveBalance()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return RedirectToAction("Login", "User");

            var leaveBalance = await _leaveBalanceService.GetLeaveBalanceByUserIdAsync(userId.Value);
            if (leaveBalance == null)
            {
                TempData["ErrorMessage"] = "Leave balance not found.";
                return RedirectToAction("Dashboard", "EmployeeLeave");
            }

            return View(leaveBalance);
        }


        // Helper Method to Get User ID
        private int? GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrEmpty(userIdClaim) ? null : int.Parse(userIdClaim);
        }
    }
}

