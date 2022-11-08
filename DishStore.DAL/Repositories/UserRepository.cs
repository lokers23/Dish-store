using DishStore.DAL.Interfaces;
using DishStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DishStore.DAL.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly DishStoreDbContext _db;
    public UserRepository(DishStoreDbContext dbContext)
    {
        _db = dbContext;
    }
    
    public IQueryable<User> GetAll()
    {
        return _db.Users;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task CreateAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }
}