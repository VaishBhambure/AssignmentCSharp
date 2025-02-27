using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentDay5.Model
{
    class AccountValidator
    {
        private static List<string> beneficiaryAccounts = new List<string>
    {
        "123456",
        "789012",
        "345678"
    };

        public static void ValidateAccount(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new ArgumentException("Account number cannot be empty.");
            }

            if (!beneficiaryAccounts.Contains(accountNumber))
            {
                throw new KeyNotFoundException("The entered account number does not exist.");
            }
        }
    }
}
