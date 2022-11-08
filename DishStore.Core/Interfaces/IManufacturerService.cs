using DishStore.Models;

namespace DishStore.Core.Interfaces;

public interface IManufacturerService
{
    public Task<List<Manufacturer>> GetManufacturersAsync();
    public Task<Manufacturer?> GetManufacturerByIdAsync(int id);
    public Task<bool> SaveManufacturerAsync(Manufacturer manufacturer);
    public Task<bool> DeleteManufacturerAsync(int id);
}