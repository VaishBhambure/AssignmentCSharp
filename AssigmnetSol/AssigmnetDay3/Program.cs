
using System.Text;

namespace AssigmnetDay3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Car mycar = new Car();
            mycar.AcceptDetails(101, "Toyoto", "Corolla", 2020, 900000);
            mycar.DisplayInfo();

            //parametrized Constutor is remaining

            Car1 mycar1 = new Car1(102, "Honda", "Corolla", 2002, 15000);
            mycar1.DisplayInfo();
        #region Password
        pass:
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            StringBuilder validationMessage = new StringBuilder();
            bool isValid = true;

            if (password.Length < 6)
            {
                validationMessage.AppendLine("Password must be at least 6 characters long.");
                isValid = false;
                goto pass;
            }

            bool hasUpperCase = false, hasDigit = false;
            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpperCase = true;
                if (char.IsDigit(c)) hasDigit = true;
            }

            if (!hasUpperCase)
            {
                validationMessage.AppendLine("Password must contain at least one uppercase letter.");
                isValid = false;
                goto pass;
            }

            if (!hasDigit)
            {
                validationMessage.AppendLine("Password must contain at least one digit.");
                isValid = false;
                goto pass;
            }

            if (isValid)
            {
                Console.WriteLine("Password is valid.");
            }
            else
            {
                Console.WriteLine("Invalid Password:");
                Console.WriteLine(validationMessage.ToString());
                goto pass;

            }

            #endregion


            
        }
    }
}
