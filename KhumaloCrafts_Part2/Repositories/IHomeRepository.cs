using Humanizer.Localisation;

namespace KhumaloCrafts_Part2.Repositories;

public interface IHomeRepository
{
    Task<IEnumerable<Product>> GetProducts(string sTerm = "", int categoryId = 0);
    Task<IEnumerable<Category>> Categories();
}
