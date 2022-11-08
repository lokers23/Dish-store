using DishStore.Core.Interfaces;
using DishStore.Models;

namespace DishStore.Core.Services;

public class DishService : IDishService
{
    public Task<Dish?> GetDishByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveDishAsync(Dish dish)
    {
        throw new NotImplementedException();
    }

    public Task<List<Dish>> GetDishesAsync()
    {
        throw new NotImplementedException();
    }

    public IQueryable<Dish> GetDishesWithFilterAsync(int? categoryId, int? cost, int? manufacturerId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteDishAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Dish>> GetDishesByCategoryId(int categoryId)
    {
        throw new NotImplementedException();
    }
}