using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentDay4.Model
{
    class UserData
    {

        public static int totalUsers;

        public string userName { get; set; }

        public  UserData(string username)
        {
            userName = username;
            totalUsers++;
        }

        public static void DisplayDetails()
        {
            
            Console.WriteLine($"Total user logged in:: {totalUsers}");
        }
    }
}
