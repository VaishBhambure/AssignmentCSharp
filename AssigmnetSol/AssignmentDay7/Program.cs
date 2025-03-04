namespace AssignmentDay7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product> {
            new Product{ProductID=101,Name="Smart watch",Category="Electronics",Price=780 },

            new Product { ProductID = 102, Name = "Laptop", Category = "Electronics", Price = 45000 },
            new Product { ProductID = 103, Name = "Headphones", Category = "Electronics", Price = 800 },
            new Product { ProductID = 104, Name = "Refrigerator", Category = "Appliances", Price = 12000},
            new Product { ProductID = 105, Name = "Smartphone", Category = "Electronics", Price = 1500 }
            };


            var filteredProduct = products.Where(p => p.Category == "Electronics" && p.Price > 1000);
            Console.WriteLine("");
            foreach (var item in filteredProduct)
            {
                Console.WriteLine($"ID::{item.ProductID}\t Name of Product::{item.Name} \t Product Category::{item.Category}\t Product Price::{item.Price}");
            }
            var maxPrice = products.Max(p => p.Price); // Get the highest price

            Console.WriteLine("Maximum Price is: Rs." + maxPrice);
        }

        class Product
        {
            public int ProductID { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public double Price { get; set; }
        }
    }
}
