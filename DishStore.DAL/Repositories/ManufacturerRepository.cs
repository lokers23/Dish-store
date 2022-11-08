using DishStore.DAL.Interfaces;
using DishStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DishStore.DAL.Repositories;

public class ManufacturerRepository : IRepository<Manufacturer>
{
    private readonly DishStoreDbContext _db;

    public ManufacturerRepository(DishStoreDbContext dbContext)
    {
        _db = dbContext;
    }
    public IQueryable<Manufacturer> GetAll()
    {
        return _db.Manufacturers;
    }

    public async Task<Manufacturer?> GetByIdAsync(int id)
    {
        var manufacturer = await _db.Manufacturers.FirstOrDefaultAsync(c => c.Id == id);
        return manufacturer;
    }

    public async Task CreateAsync(Manufacturer manufacturer)
    {
        await _db.Manufacturers.AddAsync(manufacturer);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Manufacturer manufacturer)
    {
        _db.Manufacturers.Update(manufacturer);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Manufacturer manufacturer)
    {
        _db.Manufacturers.Remove(manufacturer);
        await _db.SaveChangesAsync();
    }
}