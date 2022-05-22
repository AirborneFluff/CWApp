using AutoMapper.QueryableExtensions;

namespace API.Data.Repositorys
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly DataContext _context;
        public ProductsRepository(DataContext context)
        {
            this._context = context;
        }
        public void AddNewProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public async Task<bool> Exists(string name)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.NormalizedName == name.ToUpper());
            if (product == null) return false;
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> GetProductByName(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.NormalizedName == name.ToUpper());
        }

        public async Task<PagedList<Product>> GetProducts(PaginationParams productsParams, Func<Product, bool> predicate)
        {
            var query = _context.Products.AsQueryable();

            return await PagedList<Product>.CreateAsync(query, predicate, productsParams.PageNumber, productsParams.PageSize);
        }

        public void RemoveProduct(Product product)
        {
            _context.Remove(product);
            return;
        }
    }
}