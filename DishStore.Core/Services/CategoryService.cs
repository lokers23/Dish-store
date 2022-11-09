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
            var categories = await _categoryRepository.GetAll()
                .Include(c => c.Dishes).ToListAsync();
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

    public async Task<bool> SaveCategoryAsync(Category category)
    {
        try
        {
            if (category.Id == 0)
            {
                await _categoryRepository.CreateAsync(category);
                return true;
            }

            var categoryFromDb = await _categoryRepository.GetByIdAsync(category.Id);
            if (categoryFromDb == null)
            {
                throw new ArgumentNullException();
            }
            
            categoryFromDb.Name = category.Name;
            categoryFromDb.Dishes = category.Dishes;
            categoryFromDb.ImagePath = category.ImagePath;

            await _categoryRepository.UpdateAsync(categoryFromDb);
            
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> DeleteCategoryAsync(int id)
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