using PolicyApp.Model;


namespace PolicyApp.Interface
{
    interface IPolicy
    {

        Policy AddPolicy();
        bool UpdatePolicy();
        bool DeletePolicy();
        Policy GetPolicyByID(int id);
        List<Policy> GetAllPolicies();
        List<Policy> GetActivePolicies();

    }
}
