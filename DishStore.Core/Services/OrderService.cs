using DishStore.Core.Interfaces;
using DishStore.Models;

namespace DishStore.Core.Services;

public class OrderService : IOrderService
{
    public Task<List<Order>> GetOrdersAsync()
    {
        throw new NotImplementedException();
    }

    public IQueryable<Order> GerOrdersWithFilter(string email, string address, DateTime? startDate, DateTime? endDate)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> GetOrderByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteOrderAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveOrderAsync(Order order, List<CartItem> cartItems)
    {
        throw new NotImplementedException();
    }
}