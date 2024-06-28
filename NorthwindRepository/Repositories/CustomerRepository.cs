using NorthwindDataAccess.Model;
using NorthwindRepository.Infrastructure;

namespace NorthwindRepository.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer? GetById(string id);
    }

    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(NorthwindContext context) : base(context) { }

        public Customer? GetById(string id)
        {
            return _context.Customers.Find(id);
        }
    }
}
