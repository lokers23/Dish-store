using System.Security.Claims;
using DishStore.Models;
using DishStore.Models.ViewModels;

namespace DishStore.Core.Interfaces;

public interface IUserService
{
    public Task<List<User>> GetUsersAsync();
    public Task<User?> GetUserByIdAsync(int id);
    public Task<User?> GetUserByLoginAsync(string login);
    public Task<User> CreateUserAsync(RegisterViewModel user);
 
    public Task<bool> DeleteUserAsync(int id);
    public ClaimsIdentity Authenticate(User user);
}