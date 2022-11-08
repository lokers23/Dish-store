using DishStore.Models;
using DishStore.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DishStore.Core.Interfaces;
using DishStore.Core.Helpers;

namespace DishStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            
            var user = await _userService.CreateUserAsync(registerViewModel);
            var result = _userService.Authenticate(user);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(result));
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            
            var user = await _userService.GetUserByLoginAsync(model.Login);
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не существует!");
                return View(model);
            }

            var valueSha = Hash.HashString(model.Password);
            if (!valueSha.Equals(user.Password))
            {
                ModelState.AddModelError("", "Пароль не верный!");
                return View(model);
            }
                    
            var result = _userService.Authenticate(user);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(result));

            return RedirectToAction("Index", "Home");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
