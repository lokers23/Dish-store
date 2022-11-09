using DishStore.Core.Interfaces;
using DishStore.DAL.Interfaces;
using DishStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DishStore.Core.Services;

public class ManufacturerService : IManufacturerService
{
    private readonly IRepository<Manufacturer> _manufacturerRepository;
    public ManufacturerService(IRepository<Manufacturer> manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }
    public async Task<List<Manufacturer>> GetManufacturersAsync()
    {
        try
        {
            var manufacturers = await _manufacturerRepository.GetAll().ToListAsync();
            return manufacturers;
        }
        catch (Exception e)
        {
            return new List<Manufacturer>();
        }
    }

    public async Task<Manufacturer?> GetManufacturerByIdAsync(int id)
    {
        try
        {
            var manufacturer = await _manufacturerRepository.GetByIdAsync(id);
            return manufacturer;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<bool> SaveManufacturerAsync(Manufacturer manufacturer)
    {
        try
        {
            if (manufacturer.Id == 0)
            {
                await _manufacturerRepository.CreateAsync(manufacturer);
                return true;
            }

            var manufacturerFromDb = await _manufacturerRepository.GetByIdAsync(manufacturer.Id);
            if (manufacturerFromDb == null) 
            { 
                throw new ArgumentNullException();
            }
                
            manufacturerFromDb.Country = manufacturer.Country;
            manufacturerFromDb.Name = manufacturer.Name;
            manufacturerFromDb.Dishes = manufacturer.Dishes;

            await _manufacturerRepository.UpdateAsync(manufacturer);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> DeleteManufacturerAsync(int id)
    {
        try
        {
            var manufacturer = await _manufacturerRepository.GetByIdAsync(id);
            if (manufacturer == null)
            {
                throw new ArgumentNullException();
            }
            
            await _manufacturerRepository.DeleteAsync(manufacturer);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}