using System;

namespace LeaveManagement.Exceptions
{
    public class LeaveBalanceExceededException : Exception
    {
        public LeaveBalanceExceededException(string message) : base(message)
        {
        }
    }
}
