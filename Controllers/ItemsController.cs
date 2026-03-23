using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyCoreApp.Data;
using MyCoreApp.Models;

namespace MyCoreApp.Controllers
{
    public class ItemsController : Controller
    {
        private readonly MyAppCoreContext _context;
        public ItemsController(MyAppCoreContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var item = await _context.Items.Include(s=>s.SerialNumber)
                                            .Include(c=>c.Category)
                                            .Include(ic=>ic.ItemClients).ThenInclude(i=>i.Client)
                .ToListAsync();
            return View(item);
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId,Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId, Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemToDelete = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (itemToDelete != null)
            {
                _context.Items.Remove(itemToDelete);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Index");

        }
    }
}
