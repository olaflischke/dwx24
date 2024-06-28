using NorthwindDataAccess.Model;
using NorthwindRepository.Infrastructure;

namespace NorthwindRepository.Repositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    { }

    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(NorthwindContext context) : base(context) { }
    }
}
