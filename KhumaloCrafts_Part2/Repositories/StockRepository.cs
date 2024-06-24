using KhumaloCrafts_Part2.Data;
using KhumaloCrafts_Part2.Models.DTOs;
using KhumaloCrafts_Part2.Models;
using Microsoft.EntityFrameworkCore;

namespace KhumaloCrafts_Part2.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stock?> GetStockByProductId(int productId) => await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == productId);

        public async Task ManageStock(StockDTO stockToManage)
        {
            // if there is no stock for given product id, then add new record
            // if there is already stock for given product id, update stock's quantity
            var existingStock = await GetStockByProductId(stockToManage.ProductId);
            if (existingStock is null)
            {
                var stock = new Stock { ProductId = stockToManage.ProductId, Quantity = stockToManage.Quantity };
                _context.Stocks.Add(stock);
            }
            else
            {
                existingStock.Quantity = stockToManage.Quantity;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "")
        {
            var stocks = await (from product in _context.Products
                                join stock in _context.Stocks
                                on product.Id equals stock.ProductId
                                into product_stock
                                from productStock in product_stock.DefaultIfEmpty()
                                where string.IsNullOrWhiteSpace(sTerm) || product.ProductName.ToLower().Contains(sTerm.ToLower())
                                select new StockDisplayModel
                                {
                                    ProductId = product.Id,
                                    ProductName = product.ProductName,
                                    Quantity = productStock == null ? 0 : productStock.Quantity
                                }
                                ).ToListAsync();
            return stocks;
        }

    }

    public interface IStockRepository
    {
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task<Stock?> GetStockByProductId(int productId);
        Task ManageStock(StockDTO stockToManage);
    }
}
