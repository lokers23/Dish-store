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
        private readonly IDishService _dishService;
        private const string Success = "Авторизуйтесь перед оформлением заказа!";
        public OrderController(IOrderService orderService, IUserService userService, IDishService dishService)
        {
            _userService = userService;
            _orderService = orderService;
            _dishService = dishService;
        }

        public IActionResult Index(string email, string address, DateTime? startDate, DateTime? endDate)
        {
            var orders = _orderService.GerOrdersWithFilter(email, address, startDate,endDate);
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
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
            
            order.User = user;
            
            ModelState.Remove("User");
            if (!ModelState.IsValid) return View(order);
            order.DateOrder = DateTime.Now;

            var items = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var tasks =  items.Select(async i => new DishOrder
            {
                Dish = await _dishService.GetDishByIdAsync(i.Dish.Id) ?? new Dish(),
                Order = order,
                Count = i.Quantity
            }).ToList();
            
            var dishOrders = await Task.WhenAll(tasks);
            
            await _orderService.CreateOrderAsync(order, dishOrders.ToList());

            TempData["Success"] = "Спасибо за покупку!";
            return RedirectToAction("Index", "Home");
        }
    }
}
