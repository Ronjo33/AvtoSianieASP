using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvtoSianieASP.Data;
using AvtoSianieASP.Models;

namespace AvtoSianieASP.Controllers
{
    public class ServecesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServecesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Serveces
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Serveces.Include(s => s.Categories);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Serveces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servece = await _context.Serveces
                .Include(s => s.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servece == null)
            {
                return NotFound();
            }

            return View(servece);
        }

        // GET: Serveces/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Serveces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KatNum,DescSurves,CategoryId,Equipment,Duration,Image,Price,DateOn")] Servece servece)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servece);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", servece.CategoryId);
            return View(servece);
        }

        // GET: Serveces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servece = await _context.Serveces.FindAsync(id);
            if (servece == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", servece.CategoryId);
            return View(servece);
        }

        // POST: Serveces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KatNum,DescSurves,CategoryId,Equipment,Duration,Image,Price,DateOn")] Servece servece)
        {
            if (id != servece.Id)
            {
                return NotFound();
            }

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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", servece.CategoryId);
            return View(servece);
        }

        // GET: Serveces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servece = await _context.Serveces
                .Include(s => s.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servece == null)
            {
                return NotFound();
            }

            return View(servece);
        }

        // POST: Serveces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servece = await _context.Serveces.FindAsync(id);
            if (servece != null)
            {
                _context.Serveces.Remove(servece);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServeceExists(int id)
        {
            return _context.Serveces.Any(e => e.Id == id);
        }
    }
}
