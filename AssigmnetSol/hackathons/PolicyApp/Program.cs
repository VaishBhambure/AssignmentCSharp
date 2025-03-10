using PolicyApp.Repository;
using PolicyApp.Model;
using static PolicyApp.Model.Policy;
using System;
using PolicyApp.Interface;

namespace PolicyApp
{
    internal class Program
    {

        static IPolicy policyRepository = new PolicyRepository(); // Use interface

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n####");
                Console.WriteLine("1. Add Policy \n2. View All Policies \n3. Search Policy by ID\n4. Update Policy Details\n5. Delete Policy \n6. View Active Policies\n7. Exit");
                Console.WriteLine("Choose an Option");

                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        policyRepository.AddPolicy();
                        break;
                    case "2":
                        policyRepository.GetAllPolicies();
                        break;
                    case "3":
                        Console.Write("Enter Policy ID to search: ");
                        if (int.TryParse(Console.ReadLine(), out int searchId))
                        {
                            policyRepository.GetPolicyByID(searchId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID format. Please enter a valid integer.");
                        }
                        break;
                    case "4":
                        policyRepository.UpdatePolicy();
                        break;
                    case "5":
                        policyRepository.DeletePolicy();
                        break;
                    case "6":
                        policyRepository.GetActivePolicies();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
             }
         }

    }
}


