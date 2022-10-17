using Microsoft.AspNetCore.Mvc;
using DishStore.Models;
using DishStore.Infrastructure;
using DishStore.Models.ViewModels;

namespace DishStore.Controllers
{
    public class CartController : Controller
    {
        private DishDbContext _dbContext;
        public CartController(DishDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartView = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(s => s.Quantity * s.Dish.Cost)
            };

            return View(cartView);
        }

        public async Task<IActionResult> Add(int id)
        {
            Dish dish = await _dbContext.Dishes.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(c => c.Dish.Id == id).FirstOrDefault();

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

        public async Task<IActionResult> Decrease(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(c => c.Dish.Id == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
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

        public async Task<IActionResult> Remove(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

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
