using System;

namespace LeaveManagement.Exceptions
{
    public class LeaveRequestNotFoundException : Exception
    {
        public LeaveRequestNotFoundException(int leaveRequestId)
            : base($"Leave request with ID {leaveRequestId} was not found.")
        {
        }
        public LeaveRequestNotFoundException(string message)
            : base(message)
        {
        }
    }
}
