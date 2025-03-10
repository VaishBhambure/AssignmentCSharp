
using PolicyApp.Exceptions;
using PolicyApp.Interface;
using PolicyApp.Model;
using PolicyApp.Uitility;
using System.Data.SqlClient;
using static PolicyApp.Model.Policy;

namespace PolicyApp.Repository
{
    class PolicyRepository : IPolicy
    {

        private Dictionary<int, Policy> policies = new Dictionary<int, Policy>();
        private int nextPolicyId = 1; // Auto-increment ID

        private SqlCommand cmd;
        private string connstring;

        // **Parameterless Constructor**
        public PolicyRepository()
        {
            cmd = new SqlCommand();
            connstring = DbConnection.GetConnectionString();  // Automatically fetch connection string
        }
        public Policy AddPolicy()
        {
            try
            {
                Console.Write("Enter Policy Holder Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Policy Type (press 1-Life, 2-Health, 3-Vehicle, 4-Property): ");
                if (!Enum.TryParse(Console.ReadLine(), out PolicyType type))
                {
                    Console.WriteLine("Invalid Policy Type! Defaulting to Life.");
                    type = PolicyType.Life;
                }

                DateTime startDate = DateTime.Now.Date;
                Console.WriteLine($"Start Date is automatically set to today: {startDate:MM-dd-yyyy}");

                DateTime endDate;
                while (true)
                {
                    Console.Write("Enter End Date (MM-dd-yyyy): ");
                    if (DateTime.TryParse(Console.ReadLine(), out endDate) && endDate > startDate)
                    {
                        break;
                    }
                    Console.WriteLine("Error: End Date must be after Start Date.");
                }

                using (SqlConnection sqlConnection = new SqlConnection(connstring))
                {
                    sqlConnection.Open();
                    string query = @"
                INSERT INTO PolicyInfo (PolicyHoldersName, TypeOfPolicy, StartDate, EndDate) 
                OUTPUT INSERTED.PolicyID 
                VALUES (@Name, @Type, @StartDate, @EndDate)";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Type", (int)type);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        // Get the auto-generated Policy ID from SQL Server
                        int policyId = (int)cmd.ExecuteScalar();

                        Policy policy = new Policy(policyId, name, type, startDate, endDate);
                        Console.WriteLine($"Policy added successfully! ID: {policyId}");

                        return policy;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        public bool UpdatePolicy()
        {
            try
            {
                Console.Write("Enter Policy ID to update: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid Policy ID.");
                    return false;
                }

                using (SqlConnection sqlConnection = new SqlConnection(connstring))
                {
                    sqlConnection.Open();

                    // Check if the policy exists before attempting an update
                    string checkQuery = "SELECT COUNT(*) FROM PolicyInfo WHERE PolicyID = @PolicyID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, sqlConnection))
                    {
                        checkCmd.Parameters.AddWithValue("@PolicyID", id);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            Console.WriteLine($"Error: Policy with ID {id} not found.");
                            return false;
                        }
                    }

                    // Retrieve existing policy details
                    string selectQuery = "SELECT * FROM PolicyInfo WHERE PolicyID = @PolicyID";
                    Policy existingPolicy;
                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, sqlConnection))
                    {
                        selectCmd.Parameters.AddWithValue("@PolicyID", id);
                        using (SqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                existingPolicy = new Policy(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    (PolicyType)reader.GetInt32(2),
                                    reader.GetDateTime(3),
                                    reader.GetDateTime(4)
                                );
                            }
                            else
                            {
                                Console.WriteLine("Policy not found.");
                                return false;
                            }
                        }
                    }

                    // User input for updates
                    Console.Write("Enter new name (or press Enter to keep): ");
                    string newName = Console.ReadLine();
                    newName = string.IsNullOrWhiteSpace(newName) ? existingPolicy.PolicyHoldersName : newName;

                    Console.Write("Enter new type (1-Life, 2-Health, 3-Vehicle, 4-Property) or press Enter to keep: ");
                    string newTypeInput = Console.ReadLine();
                    PolicyType updatedType = existingPolicy.TypeOfPolicy;

                    if (!string.IsNullOrWhiteSpace(newTypeInput) && Enum.TryParse(newTypeInput, out PolicyType parsedType))
                    {
                        updatedType = parsedType;
                    }

                    DateTime updatedEndDate = existingPolicy.EndDate;
                    while (true)
                    {
                        Console.Write("Enter new End Date (MM-dd-yyyy) or press Enter to keep: ");
                        string newEndDate = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(newEndDate))
                        {
                            break; // Keep existing end date
                        }

                        if (DateTime.TryParse(newEndDate, out DateTime tempEndDate) && tempEndDate > existingPolicy.StartDate)
                        {
                            updatedEndDate = tempEndDate;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Error: End Date must be after Start Date. Try again.");
                        }
                    }

                    // Update the policy in the database
                    string updateQuery = "UPDATE PolicyInfo SET PolicyHoldersName = @Name, TypeOfPolicy = @Type, EndDate = @EndDate WHERE PolicyID = @PolicyID";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, sqlConnection))
                    {
                        updateCmd.Parameters.AddWithValue("@PolicyID", id);
                        updateCmd.Parameters.AddWithValue("@Name", newName);
                        updateCmd.Parameters.AddWithValue("@Type", (int)updatedType);
                        updateCmd.Parameters.AddWithValue("@EndDate", updatedEndDate);

                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Policy updated successfully!");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error: Policy update failed.");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


        public bool DeletePolicy()
        {
            try
            {
                Console.Write("Enter Policy ID to delete: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid ID format. Please enter a valid integer.");
                    return false;
                }

                using (SqlConnection sqlConnection = new SqlConnection(connstring))
                {
                    sqlConnection.Open();

                    // Check if policy exists before attempting deletion
                    string checkQuery = "SELECT COUNT(*) FROM PolicyInfo WHERE PolicyID = @PolicyID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, sqlConnection))
                    {
                        checkCmd.Parameters.AddWithValue("@PolicyID", id);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            Console.WriteLine($"Error: Policy with ID {id} not found.");
                            return false;
                        }
                    }

                    // Delete the policy if it exists
                    string deleteQuery = "DELETE FROM PolicyInfo WHERE PolicyID = @PolicyID";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, sqlConnection))
                    {
                        deleteCmd.Parameters.AddWithValue("@PolicyID", id);
                        int rowsAffected = deleteCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Policy deleted successfully!");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error: Policy deletion failed.");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return false;
            }
        }


        //public void GetPolicyByID()
        //{
        //    try
        //    {
        //        Console.Write("Enter Policy ID: ");
        //        int id = int.Parse(Console.ReadLine());

        //        if (!policies.TryGetValue(id, out Policy policy))
        //        {
        //            Console.WriteLine($"Error: Policy ID {id} not found.");
        //        }
        //        else
        //        {
        //            Console.WriteLine(policy);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        public Policy GetPolicyByID(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connstring))
            {
                try
                {
                    sqlConnection.Open();
                    cmd.CommandText = "SELECT * FROM PolicyInfo WHERE PolicyID = @PolicyID";
                    cmd.Parameters.Clear();  // Clear previous parameters
                    cmd.Parameters.AddWithValue("@PolicyID", id);
                    cmd.Connection = sqlConnection;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // If a record is found
                        {
                            Policy policy = new Policy(
                                reader.GetInt32(0),  // PolicyID
                                reader.GetString(1),  // PolicyHoldersName
                                (PolicyType)reader.GetInt32(2),  // TypeOfPolicy
                                reader.GetDateTime(3),  // StartDate
                                reader.GetDateTime(4)   // EndDate
                            );

                            Console.WriteLine(policy);
                            return policy;
                        }
                        else
                        {
                            Console.WriteLine($"Policy with ID {id} not found.");
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching policy: {ex.Message}");
                    return null;
                }
            }
        }

        //public List<Policy> GetAllPolicies()
        //{
        //    if (policies.Count == 0)
        //    {
        //        Console.WriteLine("No policies found.");
        //    }
        //    else
        //    {
        //        policies.Values.ToList().ForEach(Console.WriteLine);
        //    }
        //    return policies.Values.ToList();
        //}
        public List<Policy> GetAllPolicies()
        {
            List<Policy> policies = new List<Policy>();

            using (SqlConnection sqlConnection = new SqlConnection(connstring))
            {
                try
                {
                    sqlConnection.Open();
                    cmd.CommandText = "SELECT PolicyID, PolicyHoldersName, TypeOfPolicy, StartDate, EndDate FROM PolicyInfo";
                    cmd.Connection = sqlConnection;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Policy policy = new Policy(
                                reader.GetInt32(0),  // PolicyID
                                reader.GetString(1),  // PolicyHoldersName
                                (Policy.PolicyType)reader.GetInt32(2),  // TypeOfPolicy
                                reader.GetDateTime(3),  // StartDate
                                reader.GetDateTime(4)   // EndDate
                            );

                            policies.Add(policy);
                        }
                    }

                    if (policies.Count == 0)
                    {
                        Console.WriteLine("No policies found in the database.");
                    }
                    else
                    {
                        Console.WriteLine("\n===== Policy List =====");
                        policies.ForEach(Console.WriteLine);
                        Console.WriteLine("========================");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching policies: {ex.Message}");
                }
            }

            return policies;
        }
        //public List<Policy> GetActivePolicies()
        //{
        //    var activePolicies = policies.Values.Where(p => p.IsActive()).ToList();
        //    if (activePolicies.Count == 0)
        //    {
        //        Console.WriteLine("No active policies found.");
        //    }
        //    else
        //    {
        //        activePolicies.ForEach(Console.WriteLine);
        //    }
        //    return activePolicies;
        //}
        
        
    public List<Policy> GetActivePolicies()
{
    List<Policy> activePolicies = new List<Policy>();

    using (SqlConnection sqlConnection = new SqlConnection(connstring))
    {
        try
        {
            sqlConnection.Open();
            cmd.CommandText = "SELECT PolicyID, PolicyHoldersName, TypeOfPolicy, StartDate, EndDate FROM PolicyInfo WHERE StartDate <= @CurrentDate AND EndDate >= @CurrentDate";
            cmd.Parameters.Clear();  // Clear any previous parameters
            cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);
            cmd.Connection = sqlConnection;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read()) // Read all active policies
                {
                    Policy policy = new Policy(
                        reader.GetInt32(0),  // PolicyID
                        reader.GetString(1),  // PolicyHoldersName
                        (PolicyType)reader.GetInt32(2),  // TypeOfPolicy (Enum stored as int)
                        reader.GetDateTime(3),  // StartDate
                        reader.GetDateTime(4)   // EndDate
                    );

                    activePolicies.Add(policy);
                }
            }

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
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching active policies: {ex.Message}");
            return new List<Policy>();
        }
    }
}

    }

}

   
