using DishStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DishStore.Core.Interfaces;
using DishStore.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DishStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private const string Success = "Авторизуйтесь перед оформлением заказа!";
        public OrderController(IOrderService orderService, IUserService userService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        public IActionResult Index(string email, string address, DateTime? startDate, DateTime? endDate)
        {
            // IQueryable<Order> orders = _dishDbContext.Orders;
            // if (!string.IsNullOrEmpty(email))
            // {
            //     orders = orders.Where(o => o.Email.StartsWith(email));
            // }
            //
            // if (!string.IsNullOrEmpty(address))
            // {
            //     orders = orders.Where(o => o.Address.StartsWith(address));
            // }
            //
            // if (startDate != null && endDate != null)
            // {
            //     orders = orders.Where(o => o.DateOrder >= startDate && o.DateOrder <= endDate);
            // }

            var orders = _orderService.GerOrdersWithFilter(email, address, startDate,endDate);
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            
            // var order = _dishDbContext.Orders
            //     .Include(o => o.User)
            //     .Include(o => o.DishOrders)
            //     .ThenInclude(d => d.Dish)
            //     .FirstOrDefault(o => o.Id == id);
            var order = await _orderService.GetOrderByIdAsync(id);
            
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            // var order = _dishDbContext.Orders
            //     .Include(o => o.User)
            //     .Include(o => o.DishOrders)
            //     .ThenInclude(d => d.Dish)
            //     .FirstOrDefault(o => o.Id == id);
            var order = await _orderService.GetOrderByIdAsync(id);
            return View(order);
        } 
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction("Index");
        } 
        
        [HttpGet]
        public IActionResult Create()
        {
           
            if (User.Identity?.Name == null)
            {
                TempData["Success"] = Success;
                return RedirectToAction("Index", "Cart");
            }
            
            var order = new Order
            {
                PaymentMethod = "Оплата наличными"
            };
            
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (User.Identity?.Name == null)
            {
                TempData["Success"] = Success;
                return RedirectToAction("Index", "Cart");
            }
            
            var user = await _userService.GetUserByLoginAsync(User.Identity.Name);
            if (user == null)
            {
                TempData["Success"] = "Пользователя с таким логином не существует.";
                return RedirectToAction("Register", "Account");
            }
            
            //var user = _dishDbContext.Users.FirstOrDefault(u => u.Login == );
            order.User = user;
            
            ModelState.Remove("User");
            if (!ModelState.IsValid) return View(order);
            
            order.DateOrder = DateTime.Now;

            var items = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            await _orderService.SaveOrderAsync(order, items);
            // _dishDbContext.Orders.Add(order);
            // _dishDbContext.SaveChanges();
            //
            // var dishOrders = items.Select(i => new DishOrder
            // {
            //     Dish = _dishDbContext.Dishes.FirstOrDefault(d => d.Id == i.Dish.Id),
            //     Order = order,
            //     Count = i.Quantity
            // }).ToList();
            //
            // order.DishOrders = dishOrders;
            //
            // _dishDbContext.SaveChanges();

            TempData["Success"] = "Спасибо за покупку!";
            return RedirectToAction("Index", "Home");
        }
    }
}
