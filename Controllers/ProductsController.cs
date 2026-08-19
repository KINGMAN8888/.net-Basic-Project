using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data;
using ShopApp.Models;
using ShopApp.Services;

namespace ShopApp.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly AppDbContext _context;

    public ProductsController(IProductService productService, AppDbContext context)
    {
        _productService = productService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();
        return View(products);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var product = await _productService.GetByIdAsync(id.Value);
        if (product is null) return NotFound();

        return View(product);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateCategoriesAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Price,Stock,IsAvailable,CategoryId")] Product product)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(product.CategoryId);
            return View(product);
        }

        await _productService.AddAsync(product);
        TempData["Success"] = "تم إضافة المنتج بنجاح";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var product = await _productService.GetByIdAsync(id.Value);
        if (product is null) return NotFound();

        await PopulateCategoriesAsync(product.CategoryId);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Stock,IsAvailable,CategoryId")] Product product)
    {
        if (id != product.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(product.CategoryId);
            return View(product);
        }

        await _productService.UpdateAsync(product);
        TempData["Success"] = "تم تعديل المنتج";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var product = await _productService.GetByIdAsync(id.Value);
        if (product is null) return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productService.DeleteAsync(id);
        TempData["Success"] = "تم حذف المنتج";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCategoriesAsync(int? selectedId = null)
    {
        var categories = await _context.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
    }
}
