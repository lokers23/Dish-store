using DishStore.Core.Interfaces;
using DishStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace DishStore.Controllers;

public class ManufacturerController : Controller
{
    private readonly IManufacturerService _manufacturerService;
    public ManufacturerController(IManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }
    
    public async Task<IActionResult> Index()
    {
        var manufacturers = await _manufacturerService.GetManufacturersAsync();
        return View(manufacturers);
    }

    public async Task<IActionResult> Details(int id)
    {
        var manufacturer = await  _manufacturerService.GetManufacturerByIdAsync(id);
        return View(manufacturer);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Manufacturer manufacturer)
    {
        if (ModelState.IsValid)
        {
            var isSuccess = await _manufacturerService.SaveManufacturerAsync(manufacturer);
            if (isSuccess)
            {
                return RedirectToAction("Index");
            }
        }

        return View(manufacturer);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
        return View(manufacturer);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [FromForm] Manufacturer formData)
    {
        var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
        if (manufacturer == null) 
        { 
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            manufacturer.Name = formData.Name;
            manufacturer.Country = formData.Country;

            var isSuccess = await _manufacturerService.SaveManufacturerAsync(manufacturer);
        
            return RedirectToAction("Index");
        }

        return View(manufacturer);
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
        return View(manufacturer);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }
        
        var isSuccess = await _manufacturerService.DeleteManufacturerAsync(id);
        return RedirectToAction("Index");
    }
}