using DishStore.DAL.Interfaces;
using DishStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DishStore.DAL.Repositories;

public class DishRepository : IRepository<Dish>
{
    private readonly DishStoreDbContext _db;
    public DishRepository(DishStoreDbContext dbContext)
    {
        _db = dbContext;
    }
    
    public IQueryable<Dish> GetAll()
    {
        return _db.Dishes.Include(d => d.Manufacturer)
            .Include(d => d.Category);
    }

    public async  Task<Dish?> GetByIdAsync(int id)
    {
        var dish = await _db.Dishes
            .Include(d => d.Manufacturer)
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.Id == id);
        
        return dish;
    }

    public async Task CreateAsync(Dish dish)
    {
        await _db.Dishes.AddAsync(dish);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Dish dish)
    {
        _db.Dishes.Update(dish);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Dish dish)
    {
        _db.Dishes.Remove(dish);
        await _db.SaveChangesAsync();
    }
}