using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentDay6
{
    class BankTokenSytem
    {
        public Queue<int> tokenQueue = new Queue<int>();
        public int tokenNumber = 1;

        public void AddToken()
        {
            tokenQueue.Enqueue(tokenNumber);
            Console.WriteLine($"Token {tokenNumber} added to Queue");
            tokenNumber++;
        }
        public void ServeToken()
        {
            if (tokenQueue.Count > 0)
            {
                int serverToken = tokenQueue.Dequeue();
                Console.WriteLine($"Serving token is {serverToken}");
            }
            else
            {
                Console.WriteLine("No token in Queue");
            }
        }
        public void returnToken()
        {
            if (tokenQueue.Count > 0)
            {
                Console.WriteLine($"next token is : {tokenQueue.Peek()}");
            }
            else
            {
                Console.WriteLine("No token in Queue");
            }

        }
    }
}
