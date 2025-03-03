namespace AssignmentDay6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Bank System
            BankTokenSytem bank = new BankTokenSytem();
            bool isinput = true;
            while (isinput)
            {
                Console.WriteLine("\nEnter 1 to add a token, 2 to serve a token, 3 to peek next token, or 4 to exit: ");
                string input = Console.ReadLine();
                if (input == "1")
                {
                    bank.AddToken();
                }
                else if (input == "2")
                {
                    bank.ServeToken();
                }
                else if (input == "3")
                {
                    bank.returnToken();
                }
                else if (input == "4")
                {
                    isinput = false;
                }
                else
                {
                    Console.WriteLine("invalid input ,please try again");
                }


            }
            #endregion



            #region Workshop Question
            WorkshopRegistration registrationSystem = new WorkshopRegistration();

            bool running = true;
            while (running)
            {
                Console.WriteLine("\nEnter 1 to register, 2 to view registrations, or 3 to exit: ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Write("Enter workshop name: ");
                    string workshop = Console.ReadLine();
                    if (!registrationSystem.IsValidWorkshop(workshop))
                    {
                        Console.WriteLine($"Invalid workshop name: {workshop}. Available workshops: Robotics, Gen AI, ML, Cyber Security.");
                        continue;
                    }
                    Console.Write("Enter student ID: ");
                    if (int.TryParse(Console.ReadLine(), out int studentID))
                    {
                        registrationSystem.RegisterStudent(workshop, studentID);
                    }
                    else
                    {
                        Console.WriteLine("Invalid student ID. Please enter a number.");
                    }
                }
                else if (input == "2")
                {
                    Console.Write("Enter workshop name Available workshops: Robotics, Gen AI, ML, Cyber Security. ");
                    string workshop = Console.ReadLine();
                    registrationSystem.DisplayRegistrations(workshop);
                }
                else if (input == "3")
                {
                    running = false;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
            #endregion
        }

    }
}
