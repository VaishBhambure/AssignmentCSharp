using AssignmentDay5.Model;

namespace AssignmentDay5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Question 1");
            AccountValidatorRepository accountValidatorRepository = new AccountValidatorRepository();

           

            //Console.WriteLine("Available Beneficiary Accounts:");
            //foreach (var account in accountValidatorRepository.GetBeneficiaryAccountNumbers())
            //{
            //    Console.WriteLine($"- {account}");
            //}

            Console.Write("\nEnter Beneficiary Account Number: ");
            if (int.TryParse(Console.ReadLine(), out int inputAccountNumber))
            {
                if (accountValidatorRepository.ValidateAccount(inputAccountNumber))
                {
                    Console.WriteLine("Account is valid. Proceeding with the transaction...");
                }
                else
                {
                    Console.WriteLine("Transaction failed due to an invalid account number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input! Please enter a valid numeric account number.");
            }


            Console.WriteLine("Question 2");
            VehicleInsurance bikePolicy = new TwoWheelerInsurance("John Doe", "Two Wheeler", 50000);
            bikePolicy.DisplayPolicyDetails();

            VehicleInsurance carPolicy = new FourWheelerInsurance("Jane Smith","Four Wheeler", 800000);
            carPolicy.DisplayPolicyDetails();



            VehicleInsurance truckPolicy = new CommercialVehicleInsurance("Mike Johnson","Commercial ", 1200000);
            truckPolicy.DisplayPolicyDetails();



        }
    }


}
