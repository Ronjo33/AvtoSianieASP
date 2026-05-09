using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvtoSianieASP.Data;
using AvtoSianieASP.Models;
using Microsoft.AspNetCore.Authorization;

namespace AvtoSianieASP.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private static readonly List<string> WorkingHours = new()
        {
            "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00"
        };

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Customers)
                .Include(o => o.Services)
                .OrderBy(o => o.ReservationDate)
                .ThenBy(o => o.ReservationTime)
                .ToListAsync();

            return View(orders);
        }

        [Authorize]
        public async Task<IActionResult> MyReservations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _context.Orders
                .Include(o => o.Services)
                .Where(o => o.CustomerId == userId)
                .OrderByDescending(o => o.ReservationDate)
                .ThenBy(o => o.ReservationTime)
                .ToListAsync();

            return View(orders);
        }

        public IActionResult Calendar(DateTime? date, int? serveceId)
        {
            var selectedDate = (date ?? DateTime.Today).Date;
            PrepareOrderViewData(selectedDate, selectedServeceId: serveceId);

            return View(new Order
            {
                ReservationDate = selectedDate,
                ServeceId = serveceId ?? 0
            });
        }

        public async Task<IActionResult> GetBusyHours(DateTime date)
        {
            var busyHours = await _context.Orders
                .Where(o => o.ReservationDate.Date == date.Date)
                .Select(o => o.ReservationTime)
                .ToListAsync();

            return Json(busyHours);
        }

        [Authorize]
        public IActionResult Create(DateTime? date, int? serveceId)
        {
            var selectedDate = (date ?? DateTime.Today).Date;
            PrepareOrderViewData(selectedDate, selectedServeceId: serveceId);

            return View(new Order
            {
                ReservationDate = selectedDate,
                ServeceId = serveceId ?? 0,
                Massage = "Няма съобщение"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,ServeceId,Massage,ReservationDate,ReservationTime")] Order order)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            order.CustomerId = userId;
            order.DateOn = DateTime.Now;
            order.ReservationDate = order.ReservationDate.Date;

            if (string.IsNullOrWhiteSpace(order.Massage))
                order.Massage = "Няма съобщение";

            if (string.IsNullOrWhiteSpace(order.ReservationTime) || !WorkingHours.Contains(order.ReservationTime))
                ModelState.AddModelError(nameof(order.ReservationTime), "Избери валиден час.");

            if (order.ServeceId <= 0)
                ModelState.AddModelError(nameof(order.ServeceId), "Избери услуга.");

            var isTaken = await _context.Orders.AnyAsync(o =>
                o.ReservationDate.Date == order.ReservationDate.Date &&
                o.ReservationTime == order.ReservationTime);

            if (isTaken)
                ModelState.AddModelError("", "Този час вече е зает.");

            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyReservations));
            }

            PrepareOrderViewData(order.ReservationDate, selectedServeceId: order.ServeceId);
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customers)
                .Include(o => o.Services)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            PrepareOrderViewData(order.ReservationDate, order.CustomerId, order.ServeceId, order.ReservationTime);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,ServeceId,Massage,ReservationDate,ReservationTime")] Order order)
        {
            if (id != order.Id) return NotFound();

            order.DateOn = DateTime.Now;
            order.ReservationDate = order.ReservationDate.Date;

            if (string.IsNullOrWhiteSpace(order.Massage))
                order.Massage = "Няма съобщение";

            if (string.IsNullOrWhiteSpace(order.ReservationTime) || !WorkingHours.Contains(order.ReservationTime))
                ModelState.AddModelError(nameof(order.ReservationTime), "Избери валиден час.");

            var isTaken = await _context.Orders.AnyAsync(o =>
                o.Id != order.Id &&
                o.ReservationDate.Date == order.ReservationDate.Date &&
                o.ReservationTime == order.ReservationTime);

            if (isTaken)
                ModelState.AddModelError("", "Този час вече е зает.");

            if (ModelState.IsValid)
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PrepareOrderViewData(order.ReservationDate, order.CustomerId, order.ServeceId, order.ReservationTime);
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customers)
                .Include(o => o.Services)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void PrepareOrderViewData(DateTime selectedDate, string? selectedCustomerId = null, int? selectedServeceId = null, string? selectedHour = null)
        {
            var customers = _context.Users
                .Select(u => new
                {
                    u.Id,
                    DisplayName = u.Email
                })
                .ToList();

            var services = _context.Serveces
                .Select(s => new
                {
                    s.Id,
                    Name = s.DescSurves
                })
                .ToList();

            var busyHours = _context.Orders
                .Where(o => o.ReservationDate.Date == selectedDate.Date)
                .Select(o => o.ReservationTime)
                .ToList();

            ViewData["CustomerId"] = new SelectList(customers, "Id", "DisplayName", selectedCustomerId);
            ViewData["ServeceId"] = new SelectList(services, "Id", "Name", selectedServeceId);
            ViewData["Hours"] = WorkingHours;
            ViewData["BusyHours"] = busyHours;
            ViewData["SelectedHour"] = selectedHour;
        }
    }
}