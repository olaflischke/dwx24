using Microsoft.EntityFrameworkCore;
using NorthwindDataAccess.Model;
using NorthwindRepository.Infrastructure;

namespace NorthwindRepositoryUnitTests
{
    public class CustomerTests
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NorthwindREP;Integrated Security=True";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CanChangeContactName()
        {
            NorthwindContext context = GetContext(connectionString);

            UnitOfWork unitOfWork = new UnitOfWork(context);

            Customer? customer = unitOfWork.Customers.GetById("ALFKI");

            if (customer != null)
            {
                customer.ContactName = "Maria Anders";

                unitOfWork.Customers.Update(customer);
                unitOfWork.Complete();

                Assert.Pass();
            }
            else
            {
                Assert.Fail("No customer");
            }
        }

        [Test]
        public void CanDoComplexChangeAndInsert()
        {
            NorthwindContext context = GetContext(connectionString);

            UnitOfWork unitOfWork = new UnitOfWork(context);

            Customer? customer = unitOfWork.Customers.GetById("ALFKI");

            if (customer != null)
            {
                // Letzte Order des Kunden laden
                Order order = unitOfWork.Orders.GetOrdersForCustomer(customer.CustomerId, includeDetailsAndProducts: true)
                                            .OrderBy(od => od.OrderDate)
                                            .Last();

                // "Falsche" Anzahl korrigieren
                order.OrderDetails.First().Quantity = 42;

                // Neues Produkt anlegen...
                Product newProduct = new Product() { ProductName = "OLLIBO Käsebällchen", UnitPrice = 1, UnitsInStock = 1000, Discontinued = false };

                // ...und davon gleich 100 bestellen
                order.OrderDetails.Add(new OrderDetail() { Product = newProduct, Quantity = 100, UnitPrice = 1 });

                unitOfWork.Products.Add(newProduct);

                unitOfWork.Complete();

                Assert.Pass();
            }
            else
            {
                Assert.Fail("No customer");
            }
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

    }
}