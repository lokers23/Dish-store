using DishStore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DishStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DishStore.Controllers
{
    public class DishController : Controller
    {
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
        }

        public async Task<IActionResult> IndexAsync(int? category,int? cost, int? manufacturer)
        {

            var dishes = _dishService.GetDishesWithFilterAsync(category, cost, manufacturer);
            var manufacturers = new SelectList(await _manufacturerService.GetManufacturersAsync(), "Id", "Name");
            var categories = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");
            
            ViewBag.Manufacturers = manufacturers;
            ViewBag.Categories = categories;
            return View(dishes);
        }

        public async Task<IActionResult> Details(int id)
        {
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
            var dish = await _dishService.GetDishByIdAsync(id);
            return View(dish);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, IFormFile? image, Dish dish)
        {
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
            dish.ImagePath = path;
            var fullPath = _webHostEnvironment.WebRootPath + path;
            
            await Helpers.ImageHelper.SaveImage(image, fullPath);
            await _dishService.SaveDishAsync(dish);
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var dish = await _dishService.GetDishByIdAsync(id);
            return View(dish);
        }

        [HttpPost]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            await _dishService.DeleteDishAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetDishesCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Incorrect id.");
            }

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
