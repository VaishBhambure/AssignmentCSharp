using AssignmentDay5.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentDay5.Model
{
    class AccountValidatorRepository
    {
        List <int> beneficiaryAccountNumbers ;
        public AccountValidatorRepository()
        {
            beneficiaryAccountNumbers = new List<int> { 12, 456, 789 };
        }

        public bool ValidateAccount(int accountNumber)
        {
            try
            {
                if (!beneficiaryAccountNumbers.Contains(accountNumber))
                {
                    throw new InvalidAccountException($"Account number '{accountNumber}' does not exist.");
                }

                return true; // Account is valid
            }
            catch (InvalidAccountException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return false;
        }

           public List<int> GetBeneficiaryAccountNumbers()
        { 
            return beneficiaryAccountNumbers;
        }

     }
}

