using NorthwindDataAccess.Model;
using NorthwindRepository.Infrastructure;

namespace NorthwindRepository.Repositories
{
    public interface IProductRepository : IRepository<Product>
    { }

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(NorthwindContext context) : base(context) { }
    }
}
