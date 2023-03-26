using API.Core.Models;

namespace API.Core
{
    public interface IProductRepository
    {
        Task<QueryResult<Product>> GetAllProducts(ProductQuery productQuery);
        Task<Product> GetProduct(int id, bool includeRelated = true);
        Task Add(Product Product);
        Task Update(Product Product);

        void Remove(Product Product);
        void DeleteMany(List<int> ids);
    }
}