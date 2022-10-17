using DishStore.Models;
using DishStore.Models.Enum;
using DishStore.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DishStore.Infrastructure;

namespace DishStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly DishDbContext _dishDbContext;
        public AccountController(DishDbContext dishDbContext)
        {
            _dishDbContext = dishDbContext;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) 
            {
                User user = new User()
                {
                    Login = model.Login,
                    Role = (int)Role.User,
                    Password = Hash.HashString(model.Password)
                };

                _dishDbContext.Users.Add(user);
                _dishDbContext.SaveChanges();
                var result = Authenticate(user);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(result));
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _dishDbContext.Users.FirstOrDefault(u => u.Login == model.Login);
                if (user == null)
                {
                    ModelState.AddModelError("", "Пользователь не существует!");
                }
                else
                {
                    var valueSha = Hash.HashString(model.Password);
                    if (!valueSha.Equals(user.Password))
                    {
                        ModelState.AddModelError("", "Пароль не верный!");
                        return View(model);
                    }
                    
                    var result = Authenticate(user);
                    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(result));

                    return RedirectToAction("Index", "Home");
                }
            }
            
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        private ClaimsIdentity Authenticate(User user)
        {
            var role = (Role)user.Role;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString())
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
