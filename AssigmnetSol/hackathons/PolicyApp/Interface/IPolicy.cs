using PolicyApp.Model;


namespace PolicyApp.Interface
{
    interface IPolicy
    {
        //public void AddPolicy(Policy policy);
        //public void UpdatePolicy(int policyID,Policy updatedPolicy);
        //public void DeletePolicy(int policyID);
        //public Policy GetPolicyByID(int policyID);
        //public List<Policy> GetAllPolicies();
        //public List<Policy> GetActivePolicies();


        //void AddPolicy();
        //void UpdatePolicy();
        //void DeletePolicy();
        //void GetPolicyByID();
        //List<Policy> GetAllPolicies();
        //List<Policy> GetActivePolicies();

        Policy AddPolicy();
        bool UpdatePolicy();
        bool DeletePolicy();
        Policy GetPolicyByID(int id);
        List<Policy> GetAllPolicies();
        List<Policy> GetActivePolicies();

    }
}

