using DishStore.DAL.Interfaces;
using DishStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DishStore.DAL.Repositories;

public class CategoryRepository : IRepository<Category>
{
    private readonly DishStoreDbContext _db;

    public CategoryRepository(DishStoreDbContext dbContext)
    {
        _db = dbContext;
    }
    public IQueryable<Category> GetAll()
    {
        return _db.Categories;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return category;
    }

    public async Task CreateAsync(Category category)
    {
        await _db.Categories.AddAsync(category);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _db.Categories.Update(category);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
    }
}