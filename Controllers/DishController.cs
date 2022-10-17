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
        private readonly DishDbContext _dishDbContext;
        private IWebHostEnvironment _webHostEnvironment;
        public DishController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _dishDbContext = new DishDbContext();
        }

        public async Task<IActionResult> IndexAsync(int? category,int? cost, int? manufacturer)
        {
            IQueryable<Dish> dishList =  _dishDbContext.Dishes.Include(d => d.Manufacturer).Include(d => d.Category);

            if (category != null && category != 0)
            {
                dishList = dishList.Where(d => d.Category.Id == category);
            }

            if (manufacturer != null && manufacturer != 0)
            {
                dishList = dishList.Where(d => d.Manufacturer.Id == manufacturer);
            }
            
            if (cost != null)
            {
                dishList = dishList.Where(d => d.Cost <= cost);
            }
            
            var manufacturers = new SelectList(_dishDbContext.Manufacturers, "Id", "Name");
            var categories = new SelectList(_dishDbContext.Categories, "Id", "Name");
            
            ViewBag.Manufacturers = manufacturers;
            ViewBag.Categories = categories;
            
            return View(dishList);
        }

        public ActionResult Details(int id)
        {
            var dish = _dishDbContext.Dishes.Include(d => d.Manufacturer).Include(d => d.Category).FirstOrDefault(d => d.Id == id);

            return View(dish);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SelectList categories = new SelectList(_dishDbContext.Categories, "Id", "Name");
            SelectList manufacturers = new SelectList(_dishDbContext.Manufacturers, "Id", "Name");

            ViewBag.categories = categories;
            ViewBag.manufacturers = manufacturers;

            return View();
        }

        [HttpPost]
        public ActionResult Create(IFormFile Image, Dish dish)
        {
            SelectList categories = new SelectList(_dishDbContext.Categories, "Id", "Name");
            SelectList manufacturers = new SelectList(_dishDbContext.Manufacturers, "Id", "Name");

            ViewBag.categories = categories;
            ViewBag.manufacturers = manufacturers;
            
            ModelState.Remove("Manufacturer");
            
            if (Image == null)
            {
                ModelState.AddModelError("ImagePath", "Укажите изображение");
                return View(dish);
            }
            
            string path = "/img/" + Image.FileName;
            dish.ImagePath = path;

            if (!ModelState.IsValid) return View(dish);
            
            using (Stream fileStream = new FileStream(_webHostEnvironment.WebRootPath + path, FileMode.Create))
            {
                Image.CopyToAsync(fileStream);
            }
                
            _dishDbContext.Dishes.Add(dish);
            _dishDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SelectList categories = new SelectList(_dishDbContext.Categories, "Id", "Name");
            SelectList manufacturers = new SelectList(_dishDbContext.Manufacturers, "Id", "Name");

            ViewBag.categories = categories;
            ViewBag.manufacturers = manufacturers;
            var dish = _dishDbContext.Dishes.FirstOrDefault(d => d.Id == id);
            return View(dish);
        }

        [HttpPost]
        public ActionResult Edit(int id, IFormFile Image, Dish dish)
        {
            var dish_db = _dishDbContext.Dishes.FirstOrDefault(d => d.Id == id);
            if (dish_db == null)
            {
                return NotFound();
            }
            
            if (Image == null)
            {
                ModelState.AddModelError("ImagePath", "Укажите изображение");
                return View(dish);
            }
            
            const string folderRootImg = "img";
            var path = $"/{folderRootImg}/" + Image.FileName;
            using (Stream fileStream = new FileStream(_webHostEnvironment.WebRootPath + path, FileMode.Create))
            {
                Image.CopyTo(fileStream);
            }

            dish_db.ImagePath = path;
            dish_db.Name = dish.Name;
            dish_db.Material = dish.Material;
            dish_db.Cost = dish.Cost;
            dish_db.ManufacturerId = dish.ManufacturerId;
            dish_db.CategoryId = dish.CategoryId;

            _dishDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var dish = _dishDbContext.Dishes
                .Include(d => d.Category)
                .Include(d => d.Manufacturer)
                .FirstOrDefault(d => d.Id == id);
            return View(dish);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var dish = _dishDbContext.Dishes.FirstOrDefault(d => d.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            _dishDbContext.Remove(dish);
            _dishDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetDishesCategory(int Id)
        {
            bool existsCategory = await _dishDbContext.Categories.AnyAsync(c => c.Id == Id);
            if (!existsCategory)
            {
                return NotFound();
            }

            // УБРАТЬ ИНКЛУДЫ ИБО ВО ВЬЮШКИ НЕ ЮЗАЮ

            ViewBag.Category = (await _dishDbContext.Categories.FirstOrDefaultAsync(c => c.Id == Id))?.Name;
            List<Dish> dishes = await _dishDbContext.Dishes.Where(d => d.CategoryId == Id).Include(d => d.Manufacturer).Include(d => d.Category).ToListAsync<Dish>();

            return View(dishes);
        }

        public ActionResult GetAll()
        {
            var dishes = _dishDbContext.Dishes.ToList();
            return View(dishes);
        }
    }
}
