using DishStore.Models;

namespace DishStore.Core.Interfaces;

public interface IDishService
{
    public Task<Dish?> GetDishByIdAsync(int id);
    public Task<bool> SaveDishAsync(Dish dish);
    public Task<List<Dish>> GetDishesAsync();
    public IQueryable<Dish> GetDishesWithFilterAsync(int? categoryId, int? cost, int? manufacturerId);
    public Task<bool> DeleteDishAsync(int id);
    public Task<List<Dish>> GetDishesByCategoryId(int categoryId);
}