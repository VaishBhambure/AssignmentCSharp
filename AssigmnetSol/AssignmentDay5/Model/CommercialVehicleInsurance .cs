﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentDay5.Model
{
    class CommercialVehicleInsurance: VehicleInsurance
    {
        public CommercialVehicleInsurance(string policyHolderName, string vehicleType, double vehicleValue) : base(policyHolderName, vehicleType, vehicleValue)
        {
        }
        public override double CalculatePremium()
        {
            return VehicleValue * 0.08;
        }
    }
}
