using AssignmentDay5.Model;

namespace AssignmentDay5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Enter Beneficiary Account Number: ");
                string inputAccountNumber = Console.ReadLine();

                // Validate and check if account exists
                AccountValidator.ValidateAccount(inputAccountNumber);

                Console.WriteLine("Account found! Proceeding with the transaction...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Transaction process completed.");
            }
        }
    }
}
