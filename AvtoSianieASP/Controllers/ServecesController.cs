using AvtoSianieASP.Data;
using AvtoSianieASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AvtoSianieASP.Controllers
{
    public class ServecesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServecesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = _context.Serveces.Include(s => s.Categories);
            return View(await services.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var servece = await _context.Serveces
                .Include(s => s.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (servece == null) return NotFound();

            return View(servece);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,DescSurves,CategoryId,Equipment,Duration,Image,Price")] Servece servece)
        {
            if (string.IsNullOrWhiteSpace(servece.DescSurves))
                servece.DescSurves = "Нова услуга";

            if (string.IsNullOrWhiteSpace(servece.Image))
                servece.Image = "/images/homeimg.jpg";

            if (ModelState.IsValid)
            {
                var lastKatNum = await _context.Serveces
                    .OrderByDescending(s => s.Id)
                    .Select(s => s.KatNum)
                    .FirstOrDefaultAsync();

                int nextNumber = 1;

                if (!string.IsNullOrEmpty(lastKatNum) && lastKatNum.StartsWith("USL-"))
                {
                    string numPart = lastKatNum.Substring(4);

                    if (int.TryParse(numPart, out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                servece.KatNum = $"USL-{nextNumber:000}";
                servece.DateOn = DateTime.Now;

                _context.Add(servece);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", servece.CategoryId);
            return View(servece);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var servece = await _context.Serveces.FindAsync(id);

            if (servece == null) return NotFound();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", servece.CategoryId);
            return View(servece);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KatNum,DescSurves,CategoryId,Equipment,Duration,Image,Price,DateOn")] Servece servece)
        {
            if (id != servece.Id) return NotFound();

            if (string.IsNullOrWhiteSpace(servece.DescSurves))
                servece.DescSurves = "Нова услуга";

            if (string.IsNullOrWhiteSpace(servece.Image))
                servece.Image = "/images/homeimg.jpg";

            if (string.IsNullOrWhiteSpace(servece.KatNum))
                servece.KatNum = $"USL-{servece.Id:000}";

            if (servece.DateOn == default)
                servece.DateOn = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servece);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServeceExists(servece.Id))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", servece.CategoryId);
            return View(servece);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var servece = await _context.Serveces
                .Include(s => s.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (servece == null) return NotFound();

            return View(servece);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servece = await _context.Serveces.FindAsync(id);

            if (servece != null)
            {
                _context.Serveces.Remove(servece);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ServeceExists(int id)
        {
            return _context.Serveces.Any(e => e.Id == id);
        }
    }
}