using DishStore.Models;

namespace DishStore.Core.Interfaces;

public interface ICategoryService
{
    public Task<List<Category>> GetCategoriesAsync();
    public Task<Category?> GetCategoryByIdAsync(int id);
    public Task<bool> SaveCategoryAsync(Category category);
    public Task<bool> DeleteCategoryAsync(int id);
}