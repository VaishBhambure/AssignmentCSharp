namespace AssignmentDay1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter Student Details:");

            // Get Name
            Console.Write("Enter Student Name: ");
            string studentName = Console.ReadLine();

            // Get Age with Validation (Using only if-else)
            Console.Write("Enter Student Age: ");
            string inputAge = Console.ReadLine();
            int age;
            if (int.TryParse(inputAge, out age))
            {
                // Age is valid
            }
            else
            {
                Console.WriteLine("Invalid Input! Age must be a numeric value.  ");
            }

            // Get Percentage
            Console.Write("Enter Student Percentage: ");
            string inputPercentage = Console.ReadLine();
            double percentage = 0.0;
            if (double.TryParse(inputPercentage, out percentage))
            {
                // Percentage is valid
            }
            else
            {
                Console.WriteLine("Invalid Input! Percentage must be a numeric value. Defaulting to 0.");
            }
            Console.Write("Enter Student Email: ");
            string email = Console.ReadLine();

            if (email == null || email == "")
            {
                Console.WriteLine("Email cannot be empty.");
                email = "No Email Provided";
            }
            Console.WriteLine($"Name of student: {studentName}\n Age of student: {inputAge} \n Percentage of student: {inputPercentage}\n Email of student:{email}");


            #region Discharge date
            //Console.WriteLine("\nHospital Management System:");
            //Console.Write("Enter Discharge Date (or press Enter if not discharged): ");
            //string dischargeInput = Console.ReadLine();

            //// Simply check if user entered a value or not
            //if (dischargeInput != null && dischargeInput.Length>0 )
            //{
            //    Console.WriteLine($"Discharged on {dischargeInput}");
            //}
            //else
            //{
            //    Console.WriteLine("Not Discharged");

            //}

            Console.WriteLine("\nHospital Management System:");
            DateTime dischargeInput;
            bool isDate;
        Date:
            Console.Write("Enter Discharge Date (or press Enter if not discharged): ");
            //DateTime dischargeInput = Convert.ToDateTime(Console.ReadLine());
            isDate = DateTime.TryParse(Console.ReadLine(), out dischargeInput);
            // Simply check if user entered a value or not
            if (isDate == true)
            {
                Console.WriteLine($"Discharged on {dischargeInput}");
            }
            else
            {
                Console.WriteLine("Not Discharged");
                goto Date;

            }
            #endregion
        }
    }
}
