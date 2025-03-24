using LeaveManagement.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Controllers
{

    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {
        private readonly ILeaveApprovalService _leaveApprovalService;

        public AdminController(ILeaveApprovalService leaveApprovalService)
        {
            _leaveApprovalService = leaveApprovalService;
        }
        public async Task<IActionResult> AdminDashboard()
        {
            var approvals = await _leaveApprovalService.GetAllLeaveApprovalsAsync();
            return View(approvals);
        }

    }
    }

