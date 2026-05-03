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

        // 📄 СПИСЪК
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .OrderBy(o => o.ReservationDate)
                .ThenBy(o => o.ReservationTime)
                .ToListAsync();

            return View(orders);
        }
        

        // 📅 КАЛЕНДАР
        public IActionResult Calendar(DateTime? date)
        {
            var selectedDate = (date ?? DateTime.Today).Date;
            PrepareOrderViewData(selectedDate);

            return View(new Order
            {
                ReservationDate = selectedDate
            });
        }

        // 📅 ЗАЕТИ ЧАСОВЕ
        public async Task<IActionResult> GetBusyHours(DateTime date)
        {
            var busyHours = await _context.Orders
                .Where(o => o.ReservationDate.Date == date.Date)
                .Select(o => o.ReservationTime)
                .ToListAsync();

            return Json(busyHours);
        }

        // ➕ CREATE (GET)
        public IActionResult Create(DateTime? date)
        {
            var selectedDate = (date ?? DateTime.Today).Date;
            PrepareOrderViewData(selectedDate);

            return View(new Order
            {
                ReservationDate = selectedDate
            });
        }

        // ➕ CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,ServeceId,Massage,ReservationDate,ReservationTime")] Order order)
        {
            order.DateOn = DateTime.Now;
            order.ReservationDate = order.ReservationDate.Date;

            // ✔ ако няма съобщение
            if (string.IsNullOrWhiteSpace(order.Massage))
            {
                order.Massage = "Няма съобщение";
            }

            // ✔ валиден час
            if (!WorkingHours.Contains(order.ReservationTime))
            {
                ModelState.AddModelError(nameof(order.ReservationTime), "Избери валиден час.");
            }

            // ✔ зает час
            var isTaken = await _context.Orders.AnyAsync(o =>
                o.ReservationDate.Date == order.ReservationDate.Date &&
                o.ReservationTime == order.ReservationTime);

            if (isTaken)
            {
                ModelState.AddModelError("", "Този час вече е зает.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Calendar), new
                {
                    date = order.ReservationDate.ToString("yyyy-MM-dd")
                });
            }

            PrepareOrderViewData(order.ReservationDate);
            return View(order);
        }

        // ✏️ EDIT (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            PrepareOrderViewData(order.ReservationDate, order.CustomerId, order.ServeceId, order.ReservationTime);
            return View(order);
        }

        // ✏️ EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,ServeceId,Massage,ReservationDate,ReservationTime")] Order order)
        {
            if (id != order.Id) return NotFound();

            order.DateOn = DateTime.Now;
            order.ReservationDate = order.ReservationDate.Date;

            if (string.IsNullOrWhiteSpace(order.Massage))
            {
                order.Massage = "Няма съобщение";
            }

            if (!WorkingHours.Contains(order.ReservationTime))
            {
                ModelState.AddModelError(nameof(order.ReservationTime), "Избери валиден час.");
            }

            var isTaken = await _context.Orders.AnyAsync(o =>
                o.Id != order.Id &&
                o.ReservationDate.Date == order.ReservationDate.Date &&
                o.ReservationTime == order.ReservationTime);

            if (isTaken)
            {
                ModelState.AddModelError("", "Този час вече е зает.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PrepareOrderViewData(order.ReservationDate, order.CustomerId, order.ServeceId, order.ReservationTime);
            return View(order);
        }

        // 🧠 VIEW DATA
        private void PrepareOrderViewData(DateTime selectedDate, string? selectedCustomerId = null, int? selectedServeceId = null, string? selectedHour = null)
        {
            var customers = _context.Users.ToList();
            var services = _context.Serveces.ToList();

            var busyHours = _context.Orders
                .Where(o => o.ReservationDate.Date == selectedDate.Date)
                .Select(o => o.ReservationTime)
                .ToList();

            ViewData["CustomerId"] = new SelectList(customers, "Id", "Email", selectedCustomerId);
            ViewData["ServeceId"] = new SelectList(services, "Id", "DescSurves", selectedServeceId);
            ViewData["Hours"] = WorkingHours;
            ViewData["BusyHours"] = busyHours;
            ViewData["SelectedHour"] = selectedHour;
        }
    }
}