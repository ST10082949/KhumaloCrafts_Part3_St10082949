using KhumaloCrafts_Part2.Models.DTOs;
using KhumaloCrafts_Part2.Models;

namespace KhumaloCrafts_Part2.Repositories
{

    public interface ICartRepository
    {
        Task<int> AddItem(int productId, int qty);
        Task<int> RemoveItem(int productId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> DoCheckout(CheckoutModel model);


    }
}
