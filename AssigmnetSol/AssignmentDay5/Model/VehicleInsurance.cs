using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentDay5.Model
{
   abstract class VehicleInsurance
    {
        public string PolicyHolderName { get; set; }
        public string VehicleType { get; set; }
        public double VehicleValue { get; set; }

        public VehicleInsurance(string policyHolderName, string vehicleType, double vehicleValue)
        {
            PolicyHolderName = policyHolderName;
            VehicleType = vehicleType;
            VehicleValue = vehicleValue;
        }
        // Abstract method for calculating premium, to be implemented by derived classes
        public abstract double CalculatePremium();

        public void DisplayPolicyDetails()
        {
            Console.WriteLine($"Policy Holder Name: {PolicyHolderName}");
            Console.WriteLine($"Vehicle Type: {VehicleType}");
            Console.WriteLine($"Vehicle Value: {VehicleValue}");
            Console.WriteLine($"Premium: {CalculatePremium()}");
        }


    }
}
