using DishStore.Models;

namespace DishStore.Core.Interfaces;

public interface IOrderService
{
    public Task<List<Order>> GetOrdersAsync();
    public IQueryable<Order> GerOrdersWithFilter(string email, string address, 
        DateTime? startDate, DateTime? endDate);
    public Task<Order?> GetOrderByIdAsync(int id);
    public Task<bool> DeleteOrderAsync(int id);
    public Task<bool> SaveOrderAsync(Order order, List<CartItem> cartItems);
}