using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DishStore.Core.Helpers;
using DishStore.Core.Interfaces;
using DishStore.DAL.Interfaces;
using DishStore.Domain.Enum;
using DishStore.Models;
using DishStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DishStore.Core.Services;

public class UserService: IUserService
{
    private readonly IRepository<User> _userRepository;
    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<List<User>> GetUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetAll().ToListAsync();
            return users;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<User?> GetUserByLoginAsync(string login)
    {
        try
        {
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Login != null && u.Login.Equals(login));
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<User> CreateUserAsync(RegisterViewModel registerViewModel)
    {
        try
        {
            var user = new User()
            {
                Login = registerViewModel.Login,
                Role = (int)Role.User,
                Password = Hash.HashString(registerViewModel.Password)
            };

            await _userRepository.CreateAsync(user);
            return user;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            
            await _userRepository.DeleteAsync(user);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public ClaimsIdentity Authenticate(User user)
    {
        try
        {
            if (user.Role == null || user.Login == null)
            {
                throw new ValidationException("All properties must be filled in.");
            }
            
            var role = (Role)user.Role;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString())
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
        catch (Exception exception)
        {
            throw;
        }
    }
}