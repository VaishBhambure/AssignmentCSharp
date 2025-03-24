using LeaveManagementApp.Models;
using LeaveManagementApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LeaveManagementApp.Controllers
{
    [Authorize(Roles = "Employee")] // Only employees can access
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly UserManager<User> _userManager;
        public LeaveRequestController(ILeaveRequestService leaveRequestService, UserManager<User> userManager)
        {
            _leaveRequestService = leaveRequestService;
            _userManager = userManager;
        }

        // GET: Display all leave requests of logged-in employee
        public async Task<IActionResult> EmpIndex()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var leaveRequests = await _leaveRequestService.GetLeaveRequestsByUserIdAsync(userId);
            return View(leaveRequests);
        }

        // GET: Apply for leave
        [HttpGet]
        public IActionResult Apply()
        {
            return View();
        }

        // POST: Apply for leave
       

            [HttpPost]
            public async Task<IActionResult> Apply(LeaveRequest leaveRequest)
            {
                if (!ModelState.IsValid)
                {
                    return View(leaveRequest); // Return form with errors
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(leaveRequest);
                }

                // ✅ Assign Employee ID Automatically
                leaveRequest.UserId = user.Id;
                leaveRequest.Employee = user;
                leaveRequest.Status = LeaveRequest.LeaveStatus.Pending; // Default to Pending

                await _leaveRequestService.AddLeaveRequestAsync(leaveRequest);

                return RedirectToAction("EmpIndex"); // ✅ Redirect back to dashboard
            }


            // POST: Delete Leave Request (Only if Pending)
            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }
            if (leaveRequest == null || leaveRequest.Status != LeaveRequest.LeaveStatus.Pending)
            {
                return BadRequest("You can only delete pending leave requests.");
            }

            await _leaveRequestService.DeleteLeaveRequestAsync(id);
            return RedirectToAction(nameof(EmpIndex));
        }
    }
}
