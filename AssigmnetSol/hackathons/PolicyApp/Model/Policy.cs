

namespace PolicyApp.Model
{
    class Policy
    {
        public enum PolicyType
        {
            Life,
            Health,
            Vehicle,
            Property
        }

        public int PolicyID { get; set; }
        public string PolicyHoldersName { get; set; }
        public PolicyType TypeOfPolicy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Policy(int policyID, string policyHoldersName, PolicyType typeOfPolicy, DateTime startDate, DateTime endDate)
        {
            PolicyID = policyID;
            PolicyHoldersName = policyHoldersName;
            TypeOfPolicy = typeOfPolicy;
            StartDate = startDate;
            EndDate = endDate;
        }

        public bool IsActive()
        {
            return DateTime.Now >= StartDate && DateTime.Now <= EndDate;              
        }
        public override string ToString()
        {
            return $"Policy ID: {PolicyID}, Policy Holder's Name: {PolicyHoldersName}, Type of Policy: {TypeOfPolicy}, Start Date: {StartDate}, End Date: {EndDate}";
        }

    }
}
