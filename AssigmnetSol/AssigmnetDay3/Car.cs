using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssigmnetDay3
{
    class Car
    {
        public int CarID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double Price { get; set; }

        public void AcceptDetails(int carid, string brand, string model, int year, double price)
        {
            Console.WriteLine("Receiving Car Information");
            CarID = carid;
            Brand = brand;
            Model = model;
            Year = year;
            Price = price;
        }
        public void DisplayInfo()
        {
            Console.WriteLine("Presenting Car Information");
            Console.WriteLine($"Car id::{CarID}\n Car Brand::{Brand}\n Car Model::{Model}\n Car Year::{Year}\n Car Price{Price}");
        }
    }
}
