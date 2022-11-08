using DishStore.Models;

namespace DishStore.Core.Interfaces;

public interface ICategoryService
{
    public Task<List<Category>> GetCategoriesAsync();
    public Task<Category?> GetCategoryByIdAsync(int id);
    public Task<bool> SaveAsync(Category category);
    public Task<bool> DeleteAsync(int id);
}