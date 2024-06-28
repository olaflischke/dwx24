using NorthwindDataAccess.Model;
using NorthwindRepository.Repositories;

namespace NorthwindRepository.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        IProductRepository Products { get; }

        int Complete();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly NorthwindContext _context;
        public UnitOfWork(NorthwindContext context)
        {
            _context = context;

            this.Customers = new CustomerRepository(context);
            this.Orders = new OrderRepository(context);
            this.OrderDetails = new OrderDetailRepository(context);
            this.Products = new ProductRepository(context);
        }

        public ICustomerRepository Customers { get; }
        public IOrderRepository Orders { get; }
        public IOrderDetailRepository OrderDetails { get; }
        public IProductRepository Products { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
