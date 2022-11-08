namespace DishStore.DAL.Interfaces;

public interface IRepository<T>
{
    public IQueryable<T> GetAll();
    public Task<T?> GetByIdAsync(int id);
    public Task CreateAsync(T model);
    public Task UpdateAsync(T model);
    public Task DeleteAsync(T model);
}