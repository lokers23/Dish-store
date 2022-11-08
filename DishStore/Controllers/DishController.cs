using DishStore.Core.Interfaces;
using DishStore.DAL;
using DishStore.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DishStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DishStore.Controllers
{
    public class DishController : Controller
    {
        //private readonly DishStoreDbContext _dishDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDishService _dishService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        public DishController(IWebHostEnvironment webHostEnvironment, IDishService dishService, ICategoryService categoryService, IManufacturerService manufacturerService)
        {
            _webHostEnvironment = webHostEnvironment;
            _dishService = dishService;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            //_dishDbContext = new DishStoreDbContext();
        }

        public async Task<IActionResult> IndexAsync(int? category,int? cost, int? manufacturer)
        {
            //IQueryable<Dish> dishList =  _dishDbContext.Dishes
                //.Include(d => d.Manufacturer)
                //.Include(d => d.Category);
            var dishes = _dishService.GetDishesWithFilterAsync(category, cost, manufacturer);
            
            // if (category != null && category != 0)
            // {
            //     dishes = dishes.Where(d => d.Category.Id == category);
            // }
            //
            // if (manufacturer != null && manufacturer != 0)
            // {
            //     dishes = dishes.Where(d => d.Manufacturer.Id == manufacturer);
            // }
            //
            // if (cost != null)
            // {
            //     dishes = dishes.Where(d => d.Cost <= cost);
            // }
            
            // var manufacturers = new SelectList(_dishDbContext.Manufacturers, "Id", "Name");
            // var categories = new SelectList(_dishDbContext.Categories, "Id", "Name");
            var manufacturers = new SelectList(await _manufacturerService.GetManufacturersAsync(), "Id", "Name");
            var categories = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");
            
            ViewBag.Manufacturers = manufacturers;
            ViewBag.Categories = categories;
            
            return View(dishes);
        }

        public async Task<IActionResult> Details(int id)
        {
            //var dish = _dishDbContext.Dishes.Include(d => d.Manufacturer).Include(d => d.Category).FirstOrDefault(d => d.Id == id);
            var dish = await _dishService.GetDishByIdAsync(id);
            return View(dish);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var manufacturers = new SelectList(await _manufacturerService.GetManufacturersAsync(), "Id", "Name");
            var categories = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");

            ViewBag.categories = categories;
            ViewBag.manufacturers = manufacturers;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile? image, Dish dish)
        {
            var manufacturers = new SelectList(await _manufacturerService.GetManufacturersAsync(), "Id", "Name");
            var categories = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");

            ViewBag.categories = categories;
            ViewBag.manufacturers = manufacturers;
            
            ModelState.Remove("Manufacturer");
            
            if (image == null)
            {
                ModelState.AddModelError("ImagePath", "Укажите изображение");
                return View(dish);
            }
            
            var path = "/img/" + image.FileName;
            dish.ImagePath = path;

            if (!ModelState.IsValid) return View(dish);

            var fullPath = _webHostEnvironment.WebRootPath + path;
            await Helpers.ImageHelper.SaveImage(image, fullPath);
            
            await _dishService.SaveDishAsync(dish);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var manufacturers = new SelectList(await _manufacturerService.GetManufacturersAsync(), "Id", "Name");
            var categories = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");

            ViewBag.categories = categories;
            ViewBag.manufacturers = manufacturers;
            //var dish = _dishDbContext.Dishes.FirstOrDefault(d => d.Id == id);
            var dish = await _dishService.GetDishByIdAsync(id);
            return View(dish);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, IFormFile? image, Dish dish)
        {
            //var dish_db = _dishDbContext.Dishes.FirstOrDefault(d => d.Id == id);
            // var dishFromDb = await _dishService.GetDishByIdAsync(id);
            // if (dishFromDb == null)
            // {
            //     return NotFound();
            // }
            
            if (image == null)
            {
                ModelState.AddModelError("ImagePath", "Укажите изображение");
                return View(dish);
            }

            if (!ModelState.IsValid)
            {
                return View(dish);
            }

            var path =  "/img/" + image.FileName;
            var fullPath = _webHostEnvironment.WebRootPath + path;
            
            await Helpers.ImageHelper.SaveImage(image, fullPath);
            
            // dishFromDb.ImagePath = path;
            // dishFromDb.Name = dish.Name;
            // dishFromDb.Material = dish.Material;
            // dishFromDb.Cost = dish.Cost;
            // dishFromDb.ManufacturerId = dish.ManufacturerId;
            // dishFromDb.CategoryId = dish.CategoryId;
            //
            // _dishDbContext.SaveChanges();

            await _dishService.SaveDishAsync(dish);
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            // var dish = _dishDbContext.Dishes
            //     .Include(d => d.Category)
            //     .Include(d => d.Manufacturer)
            //     .FirstOrDefault(d => d.Id == id);
            var dish = await _dishService.GetDishByIdAsync(id);
            return View(dish);
        }

        [HttpPost]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            // var dish = _dishDbContext.Dishes.FirstOrDefault(d => d.Id == id);
            // if (dish == null)
            // {
            //     return NotFound();
            // }
            //
            // _dishDbContext.Remove(dish);
            // _dishDbContext.SaveChanges();
            await _dishService.DeleteDishAsync(id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetDishesCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Incorrect id.");
            }

            //ViewBag.Category = (await _dishDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id))?.Name;
            //List<Dish> dishes = await _dishDbContext.Dishes.Where(d => d.CategoryId == id).Include(d => d.Manufacturer).Include(d => d.Category).ToListAsync<Dish>();
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with id equal to {id} not exists.");
            }
            
            ViewBag.Category = category.Name;
            var dishes = await _dishService.GetDishesByCategoryId(id);
            return View(dishes);
        }

        public async Task<IActionResult> GetAll()
        {
            var dishes = await _dishService.GetDishesAsync();
            return View(dishes);
        }
    }
}
