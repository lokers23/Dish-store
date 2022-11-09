using DishStore.Core.Interfaces;
using DishStore.DAL.Interfaces;
using DishStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DishStore.Core.Services;

public class DishService : IDishService
{
    private readonly IRepository<Dish> _dishRepository;
    public DishService(IRepository<Dish> dishRepository)
    {
        _dishRepository = dishRepository;
    }
    public async Task<Dish?> GetDishByIdAsync(int id)
    {
        try
        {
            var dish = await _dishRepository.GetByIdAsync(id);
            return dish;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<bool> SaveDishAsync(Dish dish)
    {
        try
        {
            if (dish.Id == 0)
            {
                await _dishRepository.CreateAsync(dish);
                return true;
            }

            var dishFromDb = await _dishRepository.GetByIdAsync(dish.Id);
            if (dishFromDb == null)
            {
                throw new ArgumentNullException();
            }

            dishFromDb.ImagePath = dish.ImagePath;
            dishFromDb.Name = dish.Name;
            dishFromDb.Material = dish.Material;
            dishFromDb.Cost = dish.Cost;
            dishFromDb.ManufacturerId = dish.ManufacturerId;
            dishFromDb.CategoryId = dish.CategoryId;
            
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<List<Dish>> GetDishesAsync()
    {
        try
        {
            var dishes = await _dishRepository.GetAll().ToListAsync();
            return dishes;
        }
        catch (Exception e)
        {
            return new List<Dish>();
        }
    }

    public IQueryable<Dish> GetDishesWithFilterAsync(int? categoryId, int? cost, int? manufacturerId)
    {
        try
        {
            var dishes = _dishRepository.GetAll();
        
            if (categoryId != null && categoryId != 0)
            {
                dishes = dishes.Where(d => d.Category.Id == categoryId);
            }
        
            if (manufacturerId != null && manufacturerId != 0)
            {
                dishes = dishes.Where(d => d.Manufacturer.Id == manufacturerId);
            }
        
            if (cost != null)
            {
                dishes = dishes.Where(d => d.Cost <= cost);
            }

            return dishes;
        }
        catch (Exception e)
        {
            var dishes = _dishRepository.GetAll();
            return dishes;
        }
    }

    public async Task<bool> DeleteDishAsync(int id)
    {
        try
        {
            var dish = await _dishRepository.GetByIdAsync(id);
            if (dish == null)
            {
                throw new ArgumentNullException();
            }
            
            await _dishRepository.DeleteAsync(dish);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<List<Dish>> GetDishesByCategoryId(int categoryId)
    {
        try
        {
            var dishes = await _dishRepository.GetAll()
                .Where(c => c.CategoryId == categoryId)
                .ToListAsync();

            return dishes;
        }
        catch (Exception e)
        {
            return new List<Dish>();
        }
    }
}