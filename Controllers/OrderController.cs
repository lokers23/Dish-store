using DishStore.Infrastructure;
using DishStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DishStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly DishDbContext _dishDbContext;
        public OrderController(DishDbContext dishDbContext)
        {
            _dishDbContext = dishDbContext;
        }

        public IActionResult Index(string email, string address, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Order> orders = _dishDbContext.Orders;
            if (!string.IsNullOrEmpty(email))
            {
                orders = orders.Where(o => o.Email.StartsWith(email));
            }
            
            if (!string.IsNullOrEmpty(address))
            {
                orders = orders.Where(o => o.Address.StartsWith(address));
            }
            
            if (startDate != null && endDate != null)
            {
                orders = orders.Where(o => o.DateOrder >= startDate && o.DateOrder <= endDate);
            }
            
            return View(orders);
        }

        public ActionResult Details(int id)
        {
            
            var order = _dishDbContext.Orders
                .Include(o => o.User)
                .Include(o => o.DishOrders)
                .ThenInclude(d => d.Dish)
                .FirstOrDefault(o => o.Id == id);
            
            
            return View(order);
        }

        [HttpGet]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            var order = _dishDbContext.Orders
                .Include(o => o.User)
                .Include(o => o.DishOrders)
                .ThenInclude(d => d.Dish)
                .FirstOrDefault(o => o.Id == id);

            return View(order);
        } 
        
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var order = _dishDbContext.Orders.FirstOrDefault(o => o.Id == id);
            _dishDbContext.Orders.Remove(order);
            _dishDbContext.SaveChanges();
            return RedirectToAction("Index");
        } 
        
        [HttpGet]
        public IActionResult Create()
        {
            if (User.Identity.Name == null)
            {
                TempData["Success"] = "Авторизуйтесь перед оформлением заказа!";
                return RedirectToAction("Index", "Cart");
            }
            
            Order order = new Order();
            order.PaymentMethod = "Оплата наличными";
            return View(order);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            var user = _dishDbContext.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            order.User = user;
            
            ModelState.Remove("User");
            if (!ModelState.IsValid) return View(order);
            
            order.DateOrder = DateTime.Now;

            List<CartItem> items = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            _dishDbContext.Orders.Add(order);
            _dishDbContext.SaveChanges();

            List<DishOrder> dishOrders = items.Select(i => new DishOrder
            {
                Dish = _dishDbContext.Dishes.FirstOrDefault(d => d.Id == i.Dish.Id),
                Order = order,
                Count = i.Quantity
            }).ToList<DishOrder>();

            order.DishOrders = dishOrders;

            _dishDbContext.SaveChanges();

            TempData["Success"] = "Спасибо за покупку!";
            return RedirectToAction("Index", "Home");
        }
    }
}
