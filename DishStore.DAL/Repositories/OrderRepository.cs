using DishStore.DAL.Interfaces;
using DishStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DishStore.DAL.Repositories;

public class OrderRepository : IRepository<Order>
{
    private readonly DishStoreDbContext _db;

    public OrderRepository(DishStoreDbContext dbContext)
    {
        _db = dbContext;
    }
    public IQueryable<Order> GetAll()
    {
        return _db.Orders.Include(o => o.User);
    }

    public async  Task<Order?> GetByIdAsync(int id)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.DishOrders)
            .ThenInclude(d => d.Dish)
            .FirstOrDefaultAsync(d => d.Id == id);
        
        return order;
    }

    public async Task CreateAsync(Order order)
    {
        await _db.Orders.AddAsync(order);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _db.Orders.Update(order);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Order order)
    {
        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();
    }
}