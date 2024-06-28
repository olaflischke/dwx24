using Microsoft.EntityFrameworkCore;
using NorthwindDataAccess.Model;
using NorthwindRepository.Infrastructure;

namespace NorthwindRepository.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> GetOrdersForCustomer(string customerId, bool includeDetailsAndProducts = false);
    }

    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        NorthwindContext _context;
        public OrderRepository(NorthwindContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetOrdersForCustomer(string customerId, bool includeDetailsAndProducts = false)
        {
            IEnumerable<Order> orders;

            if (includeDetailsAndProducts)
            {
                orders = _context.Orders.Include(od => od.OrderDetails)
                                    .ThenInclude(od => od.Product)
                                    .Where(od => od.CustomerId == customerId);
            }
            else
            {
                orders = _context.Orders.Where(od => od.CustomerId == customerId);
            }

            return orders;
        }
    }
}
