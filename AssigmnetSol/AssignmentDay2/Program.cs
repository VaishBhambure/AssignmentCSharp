namespace AssignmentDay2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Salary Calculation System
            Console.WriteLine("Salary Calculation System ");
            Console.WriteLine("Enetr Basic salary");
            int BasicSal = Convert.ToInt32(Console.ReadLine());
            double TaxDed = BasicSal * 0.10;
            double Bonus = 0;
            Console.WriteLine("Enter the rating");
            Double rating = Convert.ToDouble(Console.ReadLine());

            if (rating >= 8)
            {
                Bonus = BasicSal * 0.20;
            }
            else if (rating >= 5)
            {
                Bonus = BasicSal * 0.10;
            }
            else
            {
                Console.WriteLine("No Bonus");
            }
            Double TotalSal = (BasicSal - TaxDed) + Bonus;
            Console.WriteLine($" computed salary::{TotalSal} ");
            #endregion

            #region train ticket booking
            int Gen_price = 200;
            int Ac_price = 1000;
            int sleeper_price = 500;

            int totalcost = 0;
            while (true)
            {
                Console.WriteLine("\nAvailable Ticket Types:");
                Console.WriteLine("1. General - 200 INR \t 2. AC - 1000 INR \t 3. Sleeper - 500 INR");
                Console.WriteLine("Type 'exit' to finish booking");

                Console.WriteLine("Enter the ticket type(general,Ac , sleeper or 'exit')");
                string ticketType = Console.ReadLine();

                if (ticketType == "exit")
                {
                    break;
                }
                Console.WriteLine("enter the number of tickets");
                int numTickets = Convert.ToInt32(Console.ReadLine());
                if (numTickets <= 0)
                {
                    Console.WriteLine("Enter valid number of tickets,please try again");
                    continue;
                }
                int cost = 0;
                switch (ticketType.ToLower())
                {
                    case "general":
                        cost = Gen_price * numTickets;
                        break;
                    case "ac":
                        cost = Ac_price * numTickets;
                        break;
                    case "sleeper":
                        cost = sleeper_price * numTickets;
                        break;
                    default:
                        Console.WriteLine("Invalid ticket type. Please try again.");
                        continue;
                }

                // Add to total cost
                totalcost += cost;
                Console.WriteLine($"Booking successful! Cost for this booking: {cost} INR");
            }

            // Display total cost at the end
            Console.WriteLine($"\nTotal cost of all bookings: {totalcost} INR");
            Console.WriteLine("Thank you for booking with us!");
            #endregion

            #region online shopping platform
            int[,] UsersData ={
                {101,690 },
                {102,908 },
                {103,300 },
                {104,500 },
                {105,1000 }
            };
            int User_id;
            bool isvalidUSer = false;
            while (!isvalidUSer)
            {
                Console.WriteLine("enter the User id");
                if (int.TryParse(Console.ReadLine(), out User_id))
                {

                    for (int i = 0; i < UsersData.GetLength(0); i++)
                    {
                        if (UsersData[i, 0] == User_id)
                        {
                            Console.WriteLine($"user {User_id} found \n Ypur wallet balance is {UsersData[i, 1]}");
                            isvalidUSer = true;
                            break;
                        }
                    }
                    if (!isvalidUSer)
                    {
                        Console.WriteLine("invalid user id");

                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a numeric User ID.\"");
                }

            }
            Console.WriteLine("thank you");
            #endregion
        }
    }
}
