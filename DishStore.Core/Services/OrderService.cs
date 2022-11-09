using DishStore.Core.Interfaces;
using DishStore.DAL.Interfaces;
using DishStore.Models;

namespace DishStore.Core.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    public OrderService(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public Task<List<Order>> GetOrdersAsync()
    {
        throw new NotImplementedException();
    }

    public IQueryable<Order> GerOrdersWithFilter(string email, string address, DateTime? startDate, DateTime? endDate)
    {
        try
        {
            var orders = _orderRepository.GetAll();
            if (!string.IsNullOrEmpty(email))
            {
                orders = orders.Where(o => o.Email.StartsWith(email));
            }
            
            if (!string.IsNullOrEmpty(address))
            {
                orders = orders.Where(o => o.Address.StartsWith(address));
            }
            
            if (startDate != null && endDate != null)
            {
                orders = orders.Where(o => o.DateOrder >= startDate && o.DateOrder <= endDate);
            }

            return orders;
        }
        catch (Exception e)
        {
            var orders = _orderRepository.GetAll();
            return orders;
        }
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new ArgumentNullException();
            }
            
            await _orderRepository.DeleteAsync(order);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> CreateOrderAsync(Order order, List<DishOrder> dishOrders)
    {
        try
        {
            await _orderRepository.CreateAsync(order);
            order.DishOrders = dishOrders;
            await _orderRepository.UpdateAsync(order);

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}