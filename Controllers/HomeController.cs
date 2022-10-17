using DishStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DishStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DishDbContext _dbContext;
        public HomeController(ILogger<HomeController> logger)
        {
            _dbContext = new DishDbContext();
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Category> catigories = _dbContext.Categories.Include(x => x.Dishes).ToList();
            return View(catigories);
        }
    }
}