using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data;
using ShopApp.Models;

namespace ShopApp.Controllers;

public class CategoriesController : Controller
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Include(c => c.Products)
            .AsNoTracking()
            .ToListAsync();

        return View(categories);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null) return NotFound();

        return View(category);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description")] Category category)
    {
        if (!ModelState.IsValid) return View(category);

        _context.Add(category);
        await _context.SaveChangesAsync();
        TempData["Success"] = "تم إضافة التصنيف بنجاح";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var category = await _context.Categories.FindAsync(id);
        if (category is null) return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedAt")] Category category)
    {
        if (id != category.Id) return NotFound();
        if (!ModelState.IsValid) return View(category);

        try
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Categories.AnyAsync(c => c.Id == id)) return NotFound();
            throw;
        }

        TempData["Success"] = "تم تعديل التصنيف";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null) return NotFound();

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category is not null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        TempData["Success"] = "تم حذف التصنيف";
        return RedirectToAction(nameof(Index));
    }
}
