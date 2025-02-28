using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentDay5.Exceptions
{
    class InvalidAccountException:Exception
    {
        public InvalidAccountException(string message) : base(message)
        {
        }
    }
}
