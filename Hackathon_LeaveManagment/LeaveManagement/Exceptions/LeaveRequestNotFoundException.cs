using System;

namespace LeaveManagement.Exceptions
{
    public class LeaveRequestNotFoundException : Exception
    {
        public LeaveRequestNotFoundException(string message) : base(message)
        {
        }
    }
}
