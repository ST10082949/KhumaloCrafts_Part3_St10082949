using Humanizer.Localisation;
using KhumaloCrafts_Part2.Data;
using Microsoft.EntityFrameworkCore;

namespace KhumaloCrafts_Part2.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> Categories()
        {
            return await _db.Categories.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts(string sTerm = "", int categoryId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _db.Products
                                                   join category in _db.Categories
                                                   on product.CategoryId equals category.Id
                                                   join stock in _db.Stocks
                                                   on product.Id equals stock.ProductId
                                                   into product_stocks
                                                   from productWithStock in product_stocks.DefaultIfEmpty()
                                                   where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.ProductName.ToLower().StartsWith(sTerm))
                                                   select new Product
                                                   {
                                                       Id = product.Id,
                                                       Image = product.Image,
                                                       ArtisanName = product.ArtisanName,
                                                       ProductName = product.ProductName,
                                                       CategoryId = product.CategoryId,
                                                       Price = product.Price,
                                                       CategoryName = category.CategoryName,
                                                       Quantity = productWithStock == null ? 0 : productWithStock.Quantity
                                                   }
                         ).ToListAsync();
            if (categoryId > 0)
            {

                products = products.Where(a => a.CategoryId == categoryId).ToList();
            }
            return products;
        }
    }
        
   }
