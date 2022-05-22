using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IProductsRepository
    {
        void AddNewProduct(Product product);
        void RemoveProduct(Product product);
        Task<PagedList<Product>> GetProducts(PaginationParams productsParams, Func<Product, bool> predicate);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductByName(string name);
        Task<Product> GetProduct(int id);
        Task<bool> Exists(string name);
    }
}