using KhumaloCrafts_Part2.Models.DTOs;
using KhumaloCrafts_Part2.Models;

namespace KhumaloCrafts_Part2.Repositories;

    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders(bool getAll = false);
        Task ChangeOrderStatus(UpdateOrderStatusModel data);
        Task TogglePaymentStatus(int orderId);
        Task<Order?> GetOrderById(int id);
        Task<IEnumerable<OrderStatus>> GetOrderStatuses();

    }
