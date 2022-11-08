using DishStore.Core.Interfaces;
using DishStore.DAL.Interfaces;
using DishStore.Models;

namespace DishStore.Core.Services;

public class ManufacturerService : IManufacturerService
{
    private readonly IRepository<Manufacturer> _manufacturerRepository;
    public ManufacturerService(IRepository<Manufacturer> manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }
    public Task<List<Manufacturer>> GetManufacturersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Manufacturer?> GetManufacturerByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveManufacturerAsync(Manufacturer manufacturer)
    {
        throw new NotImplementedException();
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