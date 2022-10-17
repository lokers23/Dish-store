using DishStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace DishStore.Controllers;

public class ManufacturerController : Controller
{
    private readonly DishDbContext _db;
    public ManufacturerController()
    {
        _db = new DishDbContext();
    }
    
    public IActionResult Index()
    {
        var manufacturers = _db.Manufacturers.ToList();
        return View(manufacturers);
    }

    public ActionResult Details(int id)
    {
        var manufacturer = _db.Manufacturers.FirstOrDefault(d => d.Id == id);
        return View(manufacturer);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Manufacturer manufacturer)
    {
        if (ModelState.IsValid)
        {
            _db.Manufacturers.Add(manufacturer);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(manufacturer);
    }

    public ActionResult Edit(int id)
    {
        var manufacturer = _db.Manufacturers.FirstOrDefault(d => d.Id == id);
        return View(manufacturer);
    }

    [HttpPost]
    public ActionResult Edit(int id, [FromForm]Manufacturer formData)
    {
        var manufacturer = _db.Manufacturers.FirstOrDefault(d => d.Id == id);
        if (manufacturer == null) 
        { 
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            manufacturer.Name = formData.Name;
            manufacturer.Country = formData.Country;
        
            _db.SaveChanges();
        
            return RedirectToAction("Index");
        }

        return View(manufacturer);
    }
    
    //[Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        var manufacturer = _db.Manufacturers.FirstOrDefault(m => m.Id == id);
        return View(manufacturer);
    }

    [HttpPost]
    //[Authorize(Roles ="Admin")]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        var manufacturer = _db.Manufacturers.FirstOrDefault(d => d.Id == id);
        if (manufacturer == null)
        {
            return NotFound();
        }

        _db.Remove(manufacturer);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}