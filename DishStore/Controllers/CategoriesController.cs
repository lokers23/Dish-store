using DishStore.Core.Interfaces;
using DishStore.DAL.Interfaces;
using DishStore.Helpers;
using Microsoft.AspNetCore.Mvc;

using DishStore.Models;

namespace DishStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoriesController(IWebHostEnvironment webHostEnvironment, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories =  await _categoryService.GetCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id = 0)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Incorrect id for category.");
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile? image, [Bind("Id,Name,ImagePath")] Category category)
        {
            if (image == null)
            {
                ModelState.AddModelError("ImagePath", "Select image.");
                return View(category);
            }
            
            var path = "/img/" + image.FileName;
            category.ImagePath = path;

            if (!ModelState.IsValid)
            {
                return View(category);
            }
            
            var fullPath = _webHostEnvironment.WebRootPath + path;
                
            await ImageHelper.SaveImage(image, fullPath);
            await _categoryService.SaveCategoryAsync(category);
                
            return RedirectToAction("Index");
           
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            
            return View(category);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile? image, [Bind("Id,Name,ImagePath")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (image == null)
            {
                ModelState.AddModelError("ImagePath", "Select image.");
                return View(category);
            }
            
            var path = "/img/" + image.FileName;
            category.ImagePath = path;
            
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            
            var fullPath = _webHostEnvironment.WebRootPath + path;
                
            await ImageHelper.SaveImage(image, fullPath);
            var isSuccess =  await _categoryService.SaveCategoryAsync(category);
            if (!isSuccess)
            {
                return StatusCode(500, "Server error. Try again update category.");
            }
                
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("The category id can not be null.");
            }
            
            var category = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound($"Can not find category with id equal to {id}.");
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isDelete =  await _categoryService.DeleteCategoryAsync(id);
            if (!isDelete)
            {
                return BadRequest("Throw exception on server. Try delete category again.");
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
