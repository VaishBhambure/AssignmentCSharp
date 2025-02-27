using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentDay4.Model
{
    class Employee
    {
         
    public string Name { get; set; }
    public double Salary { get; set; }
     public Employee(string name,
         double salary)
        {
            Name = name;
            Salary = salary;
        }
     public virtual  void DisplayDetails()
        {
            Console.WriteLine($"Name of EMployee::{Name} Salary of employee::{Salary}");
        }
    }
}
