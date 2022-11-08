using DishStore.Core.Interfaces;
using DishStore.Helpers;
using Microsoft.AspNetCore.Mvc;
using DishStore.Models;
using DishStore.Models.ViewModels;

namespace DishStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IDishService _dishService;
        public CartController(IDishService dishService)
        {
            _dishService = dishService;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartView = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(s => s.Quantity * s.Dish.Cost)
            };

            return View(cartView);
        }

        public async Task<IActionResult> Add(int id)
        {
            //Dish dish = await _dbContext.Dishes.FindAsync(id);
            var dish = await _dishService.GetDishByIdAsync(id);
            if (dish == null)
            {
                return RedirectToAction("Index");
            }
            
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var cartItem = cart.FirstOrDefault(c => c.Dish.Id == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem(dish));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            TempData["Success"] = "Товар добавлен в корзину!";

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Decrease(int id)
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            if (cart == null)
            {
                return  RedirectToAction("Index");
            }
            
            var cartItem = cart.FirstOrDefault(c => c.Dish.Id == id);

            if (cartItem?.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.Dish.Id == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "Товар удалён из корзины!";

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            if (cart == null)
            {
                return  RedirectToAction("Index");
            }
            
            cart.RemoveAll(p => p.Dish.Id == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "Товар удалён из корзины!";

            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index");
        }
    }
}
