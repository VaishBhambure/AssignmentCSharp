using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AssigmnetDay3
{
    class Car1
    {
        private int carId;
        private string brand;
        private string model;
        private int year;
        private double price;

        public int CarID { get=>carId; set=>carId=value; }
        public string Brand { get=>brand; set=>brand=value; }
        public string Model { get=>model; set=>model=value; }
        public int Year { get => year; set => year = value; }
        public double Price { get=>price; set=>price=value; }

        public Car1(int carId, string brand, string model, int year, double price)
        {
            Console.WriteLine("Receiving Car Information");
            CarID = carId;
            Brand = brand;
            Model = model;
            Year = year;
            Price = price;
        }
        
        public void DisplayInfo()
        {
            Console.WriteLine("Presenting Car Information");
            Console.WriteLine($"Car id::{CarID}\n Car Brand::{Brand}\n Car Model::{Model}\n Car Year::{Year}\n Car Price::{Price}");
        }

    }
}
