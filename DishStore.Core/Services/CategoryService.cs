using System.ComponentModel.DataAnnotations;
using DishStore.Core.Interfaces;
using DishStore.DAL.Interfaces;
using DishStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DishStore.Core.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    public CategoryService(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<List<Category>> GetCategoriesAsync()
    {
        try
        {
            var categories = await _categoryRepository.GetAll().ToListAsync();
            return categories;
        }
        catch (Exception exception)
        {
            return new List<Category>();
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                throw new ValidationException($"An id equal to {id} is invalid.");
            }
            
            var category = await _categoryRepository.GetByIdAsync(id);
            return category;
        }
        catch (Exception exception)
        {
            return null;
        }
    }

    public async Task<bool> SaveAsync(Category category)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                throw new ValidationException($"An id equal to {id} is invalid.");
            }
            
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category) ,"Can not delete category equal to null.");
            }

            await _categoryRepository.DeleteAsync(category);
            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
    }
}