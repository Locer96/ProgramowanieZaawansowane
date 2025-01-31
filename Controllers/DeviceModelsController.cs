using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryApp.Data;
using InventoryApp.Models;
using System.Text.RegularExpressions;

namespace InventoryApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DeviceModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeviceModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DeviceModels
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["TypeSortParm"] = sortOrder == "type" ? "type_desc" : "type";
            ViewData["AssignedWorkStationsCountSortParm"] = sortOrder == "assigned_workstations_count" ? "assigned_workstations_count_desc" : "assigned_workstations_count";

            var deviceModels = _context.DeviceModels.AsQueryable();

            var deviceModelsList = await deviceModels.AsNoTracking().ToListAsync();

            switch (sortOrder)
            {
                case "name_desc":
                    deviceModelsList = deviceModelsList.OrderByDescending(dm => Regex.Replace(dm.Name ?? "", @"\d+", match => match.Value.PadLeft(10, '0'))).ToList();
                    break;
                case "type":
                    deviceModelsList = deviceModelsList.OrderBy(dm => Regex.Replace(dm.Type ?? "", @"\d+", match => match.Value.PadLeft(10, '0'))).ToList();
                    break;
                case "type_desc":
                    deviceModelsList = deviceModelsList.OrderByDescending(dm => Regex.Replace(dm.Type ?? "", @"\d+", match => match.Value.PadLeft(10, '0'))).ToList();
                    break;
                case "assigned_workstations_count":
                    deviceModelsList = deviceModelsList.OrderBy(dm => _context.WorkStations.Count(ws => ws.PC == dm.Name || ws.Display == dm.Name)).ToList();
                    break;
                case "assigned_workstations_count_desc":
                    deviceModelsList = deviceModelsList.OrderByDescending(dm => _context.WorkStations.Count(ws => ws.PC == dm.Name || ws.Display == dm.Name)).ToList();
                    break;
                default:
                    deviceModelsList = deviceModelsList.OrderBy(dm => Regex.Replace(dm.Name ?? "", @"\d+", match => match.Value.PadLeft(10, '0'))).ToList();
                    break;
            }

            return View(deviceModelsList.Select(dm => new DeviceModel
            {
                Id = dm.Id,
                Name = dm.Name,
                Type = dm.Type,
                AssignedWorkStationsCount = _context.WorkStations.Count(ws => ws.PC == dm.Name || ws.Display == dm.Name)
            }));
        }

        // GET: DeviceModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceModel = await _context.DeviceModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deviceModel == null)
            {
                return NotFound();
            }

            return View(deviceModel);
        }

        // GET: DeviceModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeviceModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type")] DeviceModel deviceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deviceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deviceModel);
        }

        // GET: DeviceModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceModel = await _context.DeviceModels.FindAsync(id);
            if (deviceModel == null)
            {
                return NotFound();
            }
            return View(deviceModel);
        }

        // POST: DeviceModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type")] DeviceModel deviceModel)
        {
            if (id != deviceModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deviceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceModelExists(deviceModel.Id))
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
            return View(deviceModel);
        }

        // GET: DeviceModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceModel = await _context.DeviceModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deviceModel == null)
            {
                return NotFound();
            }

            return View(deviceModel);
        }

        // POST: DeviceModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deviceModel = await _context.DeviceModels.FindAsync(id);
            _context.DeviceModels.Remove(deviceModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceModelExists(int id)
        {
            return _context.DeviceModels.Any(e => e.Id == id);
        }
    }
}
