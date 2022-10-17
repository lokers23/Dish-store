using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DishStore.Models;

namespace DishStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DishDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        public CategoriesController(DishDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Categories != null ? 
                          View(await _context.Categories.ToListAsync()) :
                          Problem("Entity set 'DishDbContext.Categories'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile Image, [Bind("Id,Name,ImagePath")] Category category)
        {
            if (Image == null)
            {
                ModelState.AddModelError("ImagePath", "Укажите изображение");
                return View(category);
            }
            
            string path = "/img/" + Image.FileName;
            category.ImagePath = path;

            if (ModelState.IsValid)
            {
                using (var fileStream = new FileStream(_webHostEnvironment.WebRootPath + path, FileMode.Create))
                {
                    Image.CopyToAsync(fileStream);
                }

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile Image, [Bind("Id,Name,ImagePath")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (Image == null)
            {
                ModelState.AddModelError("ImagePath", "Укажите изображение");
                return View(category);
            }
            
            string path = "/img/" + Image.FileName;
            category.ImagePath = path;
            
            if (ModelState.IsValid)
            {
                try
                {
                    using (var fileStream = new FileStream(_webHostEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        Image.CopyToAsync(fileStream);
                    }
                    
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'DishDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
