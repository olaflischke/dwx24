using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NorthwindDataAccess.Model;

namespace NorthwindDataAccessUnitTests
{

    public class CustomerTests
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NorthwindDA;Integrated Security=True";

        [SetUp]
        public void Setup()
        {
            // ResetData();
        }

        [Test]
        public void CanChangeContactName()
        {
            NorthwindContext context = GetContext(connectionString);

            Customer? alfki = context.Customers.Find("ALFKI");

            if (alfki != null)
            {
                alfki.ContactName = "Maria Schmitt";

                context.SaveChanges();

                Assert.Pass();
            }
            else
            {
                Assert.Fail("No customer 'ALFKI' found.");
            }
        }

        [Test]
        public void CanDoComplexChangeAndInsert()
        {
            NorthwindContext context = GetContext(connectionString);

            Customer? customer = context.Customers.Find("ALFKI");

            if (customer != null)
            {
                // Letzte Order von ALFKI laden
                Order order = context.Orders.Include(od => od.OrderDetails)
                                                .Where(od => od.Customer == customer)
                                                .OrderBy(od => od.OrderDate)
                                                .Last();

                // Alle Produkte laden (ineffizient!)
                List<Product> allProducts = context.Products.ToList();

                // "Falsche" Anzahl korrigieren
                order.OrderDetails.First().Quantity = 42;

                // Neues Produkt anlegen...
                Product newProduct = new Product() { ProductName = "OLLIBO Käsebällchen", UnitPrice = 1, UnitsInStock = 1000, Discontinued = false };

                // ...und davon gleich 100 bestellen
                order.OrderDetails.Add(new OrderDetail() { Product = newProduct, Quantity = 100, UnitPrice = 1 });

                // Neues Produkt dem Context hinzufügen
                context.Products.Add(newProduct);

                // Alles speichern
                context.SaveChanges();

                Assert.Pass();
            }
            else
            {
                Assert.Fail("No customer 'ALFKI' found.");
            }

        }

        private void ResetData()
        {
            Console.WriteLine("Starting data reset...");

            NorthwindContext context = GetContext(connectionString);

            Customer alfki = context.Customers.Find("ALFKI");
            alfki.CompanyName = "Alfreds Futterkiste";
            alfki.ContactName = "Maria Anders";

            context.SaveChanges();

            int delayedOrders = context.Orders
                            .Where(od => od.ShippedDate.Value.Year == 2008)
                            .ExecuteUpdate(s => s.SetProperty(od => od.ShippedDate, od => od.ShippedDate.Value.AddYears(-10)));

            Console.WriteLine("...data reset done.");
        }


        private NorthwindContext GetContext(string connectionString, bool logging = true)
        {
            DbContextOptionsBuilder<NorthwindContext> builder = new DbContextOptionsBuilder<NorthwindContext>()
                                                                    .UseSqlServer(connectionString);

            if (logging) builder.LogTo(logString => LogIt(logString), Microsoft.Extensions.Logging.LogLevel.Information);

            return new NorthwindContext(builder.Options);
        }

        private void LogIt(string logString)
        {
            Console.WriteLine(logString);
        }

        private NorthwindContext GetContextWithoutTracking(string connectionString)
        {
            DbContextOptions<NorthwindContext> options = (new DbContextOptionsBuilder<NorthwindContext>())
                .UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            return new NorthwindContext(options);

        }
    }
}