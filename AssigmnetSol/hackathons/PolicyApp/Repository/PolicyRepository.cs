
using PolicyApp.Exceptions;
using PolicyApp.Interface;
using PolicyApp.Model;
using static PolicyApp.Model.Policy;

namespace PolicyApp.Repository
{
    class PolicyRepository : IPolicy
    {
        //private Dictionary<int, Policy> policies = new Dictionary<int, Policy>();

        //public void AddPolicy(Policy policy)
        //{
        //    if (policies.ContainsKey(policy.PolicyID))
        //    {
        //        throw new PolicyNotFoundException("Policy ID already exists");
        //    }
        //    policies.Add(policy.PolicyID, policy);
        //}

        //public void UpdatePolicy(int policyID, Policy updatedPolicy)
        //{
        //    if (!policies.ContainsKey(policyID))
        //    {
        //        throw new PolicyNotFoundException($"Policy ID{policyID} does not exist");
        //    }
        //    policies[policyID] = updatedPolicy;
        //}

        //public void DeletePolicy(int policyID)
        //{
        //    if (!policies.Remove(policyID))
        //    {
        //        throw new PolicyNotFoundException($"Policy ID{policyID} does not exist");
        //    }

        //}

        //public Policy GetPolicyByID(int policyID)
        //{
        //    if (!policies.TryGetValue(policyID, out Policy policy))
        //    {
        //        throw new PolicyNotFoundException($"Error: Policy ID {policyID} not found.");
        //    }

        //    return policy;
        //}

        //public List<Policy> GetAllPolicies()
        //{
        //    //return policies.Values.ToList();
        //    return new List<Policy>(policies.Values);
        //}
        //public List<Policy> GetActivePolicies() 
        //{
        //    return new List<Policy> (policies.Values.Where(p => p.IsActive()));

        //}


        private Dictionary<int, Policy> policies = new Dictionary<int, Policy>();
        private int nextPolicyId = 1; // Auto-increment ID

        public void AddPolicy()
        {
            try
            {
                int id = nextPolicyId++;  // Auto-increment ID
                Console.WriteLine($"Generated Policy ID: {id}");

                Console.Write("Enter Policy Holder Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Policy Type (Life, Health, Vehicle, Property): ");
                PolicyType type;
                while (!Enum.TryParse(Console.ReadLine(), true, out type))
                {
                    Console.WriteLine("Invalid Policy Type! Please enter Life, Health, Vehicle, or Property:");
                }

                //  Set Start Date to the Current Date (User cannot change it)
                DateTime startDate = DateTime.Now.Date;
                Console.WriteLine($"Start Date is automatically set to today: {startDate:MM-dd-yyyy}");

                DateTime endDate;
                while (true)  //Keep asking until a valid End Date is entered
                {
                    Console.Write("Enter End Date (MM-dd-yyyy): ");
                    if (DateTime.TryParse(Console.ReadLine(), out endDate))
                    {
                        if (endDate > startDate) // Ensure End Date is after Start Date
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Error: End Date must be after Start Date. Please enter again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format! Please enter in MM-dd-yyyy format.");
                    }
                }

                
                Policy policy = new Policy(id, name, type, startDate, endDate);
                policies.Add(id, policy);
                Console.WriteLine("Policy added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }


        }

        public void UpdatePolicy()
        {
            try
            {
                Console.Write("Enter Policy ID to update: ");
                int id = int.Parse(Console.ReadLine());

                if (!policies.ContainsKey(id))
                {
                    Console.WriteLine($"Policy with ID {id} not found.");
                    return;
                }

                Policy existingPolicy = policies[id];

                Console.WriteLine("Select fields to update (press Enter to skip a field):");

                Console.Write($"Current Name: {existingPolicy.PolicyHoldersName} | Enter new name (or press Enter to keep): ");
                string newName = Console.ReadLine();
                newName = string.IsNullOrWhiteSpace(newName) ? existingPolicy.PolicyHoldersName : newName;

                Console.Write($"Current Type: {existingPolicy.TypeOfPolicy} | Enter new type (Life, Health, Vehicle, Property) or press Enter to keep: ");
                string newType = Console.ReadLine();
                PolicyType updatedType = string.IsNullOrWhiteSpace(newType) ? existingPolicy.TypeOfPolicy : (PolicyType)Enum.Parse(typeof(PolicyType), newType, true);

                Console.Write($"Current End Date: {existingPolicy.EndDate:MM-dd-yyyy} | Enter new End Date (MM-dd-yyyy) or press Enter to keep: ");
                string newEndDate = Console.ReadLine();
                DateTime updatedEndDate = string.IsNullOrWhiteSpace(newEndDate) ? existingPolicy.EndDate : DateTime.Parse(newEndDate);

                policies[id] = new Policy(id, newName, updatedType, existingPolicy.StartDate, updatedEndDate);
                Console.WriteLine("Policy updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void DeletePolicy()
        {
            try
            {
                Console.Write("Enter Policy ID to delete: ");
                int id = int.Parse(Console.ReadLine());

                if (!policies.Remove(id))
                {
                    Console.WriteLine($"Policy with ID {id} not found.");
                }
                else
                {
                    Console.WriteLine("Policy deleted successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetPolicyByID()
        {
            try
            {
                Console.Write("Enter Policy ID: ");
                int id = int.Parse(Console.ReadLine());

                if (!policies.TryGetValue(id, out Policy policy))
                {
                    Console.WriteLine($"Error: Policy ID {id} not found.");
                }
                else
                {
                    Console.WriteLine(policy);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<Policy> GetAllPolicies()
        {
            if (policies.Count == 0)
            {
                Console.WriteLine("No policies found.");
            }
            else
            {
                policies.Values.ToList().ForEach(Console.WriteLine);
            }
            return policies.Values.ToList();
        }

        public List<Policy> GetActivePolicies()
        {
            var activePolicies = policies.Values.Where(p => p.IsActive()).ToList();
            if (activePolicies.Count == 0)
            {
                Console.WriteLine("No active policies found.");
            }
            else
            {
                activePolicies.ForEach(Console.WriteLine);
            }
            return activePolicies;
        }
    }
}

   
