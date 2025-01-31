using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryApp.Data;
using InventoryApp.Models;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace InventoryApp.Controllers
{
    [Authorize(Roles = "Support,Administrator")]
    public class WorkStationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkStationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkStations
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["WorkStationNumberSortParm"] = String.IsNullOrEmpty(sortOrder) ? "workstation_number_desc" : "";
            ViewData["UserEmailSortParm"] = sortOrder == "user_email" ? "user_email_desc" : "user_email";
            ViewData["PCSerialNumberSortParm"] = sortOrder == "pc_serial_number" ? "pc_serial_number_desc" : "pc_serial_number";
            ViewData["PCSortParm"] = sortOrder == "pc" ? "pc_desc" : "pc";
            ViewData["DisplaySortParm"] = sortOrder == "display" ? "display_desc" : "display";
            ViewData["KeyboardSortParm"] = sortOrder == "keyboard" ? "keyboard_desc" : "keyboard";
            ViewData["MouseSortParm"] = sortOrder == "mouse" ? "mouse_desc" : "mouse";

            var workStations = _context.WorkStations
                .Include(ws => ws.UserWorkStation)
                .ThenInclude(uw => uw.User)
                .AsQueryable();

            var workStationsList = await workStations.AsNoTracking().ToListAsync();

            switch (sortOrder)
            {
                case "workstation_number_desc":
                    workStationsList = workStationsList.OrderByDescending(ws => Regex.Replace(ws.WorkStationNumber ?? "", @"\d+", match => match.Value.PadLeft(10, '0'))).ToList();
                    break;
                case "user_email":
                    workStationsList = workStationsList.OrderBy(ws => Regex.Replace(ws.UserWorkStation?.User?.Email ?? "", @"\d+", match => match.Value.PadLeft(10, '0'))).ToList();
                    break;
                case "user_email_desc":
                    workStationsList = workStationsList.OrderByDescending(ws => Regex.Replace(ws.UserWorkStation?.User?.Email ?? "", @"\d+", match => match.Value.PadLeft(10, '0'))).ToList();
                    break;
                case "pc_serial_number":
                    workStationsList = workStationsList.OrderBy(ws => ws.PCSerialNumber).ToList();
                    break;
                case "pc_serial_number_desc":
                    workStationsList = workStationsList.OrderByDescending(ws => ws.PCSerialNumber).ToList();
                    break;
                case "pc":
                    workStationsList = workStationsList.OrderBy(ws => ws.PC).ToList();
                    break;
                case "pc_desc":
                    workStationsList = workStationsList.OrderByDescending(ws => ws.PC).ToList();
                    break;
                case "display":
                    workStationsList = workStationsList.OrderBy(ws => ws.Display).ToList();
                    break;
                case "display_desc":
                    workStationsList = workStationsList.OrderByDescending(ws => ws.Display).ToList();
                    break;
                case "keyboard":
                    workStationsList = workStationsList.OrderBy(ws => ws.Keyboard).ToList();
                    break;
                case "keyboard_desc":
                    workStationsList = workStationsList.OrderByDescending(ws => ws.Keyboard).ToList();
                    break;
                case "mouse":
                    workStationsList = workStationsList.OrderBy(ws => ws.Mouse).ToList();
                    break;
                case "mouse_desc":
                    workStationsList = workStationsList.OrderByDescending(ws => ws.Mouse).ToList();
                    break;
                default:
                    workStationsList = workStationsList.OrderBy(ws => Regex.Replace(ws.WorkStationNumber ?? "", @"\d+", match => match.Value.PadLeft(10, '0'))).ToList();
                    break;
            }

            return View(workStationsList);
        }

        // GET: WorkStations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workStation = await _context.WorkStations
                .Include(ws => ws.UserWorkStation)
                .ThenInclude(uw => uw.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workStation == null)
            {
                return NotFound();
            }

            return View(workStation);
        }

        // GET: WorkStations/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: WorkStations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkStationNumber,PCSerialNumber,PC,Display,Keyboard,Mouse,AdditionalInfo,UserWorkStation")] WorkStation workStation)
        {
            if (workStation.UserWorkStation?.User?.Email == "None")
            {
                workStation.UserWorkStation = null;
            }
            if(workStation.PC == "None")
            {
                workStation.PC = null;
            }
            if (workStation.Display == "None")
            {
                workStation.Display = null;
            }

            if (workStation.UserWorkStation?.User?.Email != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == workStation.UserWorkStation.User.Email);
                if (user != null)
                {
                    workStation.UserWorkStation.UserId = user.Id;
                    workStation.UserWorkStation.WorkStationId = workStation.Id;

                    var existingUserWorkStation = await _context.AspNetUserWorkStations
                        .Include(uw => uw.WorkStation)
                        .FirstOrDefaultAsync(uw => uw.UserId == user.Id);
                    if (existingUserWorkStation != null)
                    {
                        ModelState.AddModelError("UserWorkStation.User.Email", $"This user is already assigned to workstation {existingUserWorkStation.WorkStation.WorkStationNumber}.");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserWorkStation.User.Email", "User not found.");
                }
            }

            if (await _context.WorkStations.AnyAsync(ws => ws.WorkStationNumber == workStation.WorkStationNumber))
            {
                ModelState.AddModelError("WorkStationNumber", "A workstation with this number already exists.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(workStation);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    ModelState.AddModelError("WorkStationNumber", "A workstation with this number already exists.");
                }
            }

            return View(workStation);
        }

        // GET: WorkStations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workStation = await _context.WorkStations
                .Include(ws => ws.UserWorkStation)
                .FirstOrDefaultAsync(ws => ws.Id == id);
            if (workStation == null)
            {
                return NotFound();
            }

            return View(workStation);
        }

        // POST: WorkStations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkStationNumber,PCSerialNumber,PC,Display,Keyboard,Mouse,AdditionalInfo,UserWorkStation")] WorkStation workStation)
        {
            if (id != workStation.Id)
            {
                return NotFound();
            }

            if (workStation.UserWorkStation?.User?.Email == "None")
            {
                workStation.UserWorkStation = null;
            }
            if (workStation.PC == "None")
            {
                workStation.PC = null;
            }
            if (workStation.Display == "None")
            {
                workStation.Display = null;
            }

            if (workStation.UserWorkStation?.User?.Email != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == workStation.UserWorkStation.User.Email);
                if (user != null)
                {
                    var existingUserWorkStation = await _context.AspNetUserWorkStations
                        .FirstOrDefaultAsync(uw => uw.WorkStationId == workStation.Id);

                    if (existingUserWorkStation != null)
                    {
                        _context.AspNetUserWorkStations.Remove(existingUserWorkStation);
                        await _context.SaveChangesAsync();
                    }

                    workStation.UserWorkStation = new AspNetUserWorkStation
                    {
                        UserId = user.Id,
                        WorkStationId = workStation.Id
                    };

                    var conflictingUserWorkStation = await _context.AspNetUserWorkStations
                        .Include(uw => uw.WorkStation)
                        .FirstOrDefaultAsync(uw => uw.UserId == user.Id && uw.WorkStationId != workStation.Id);
                    if (conflictingUserWorkStation != null)
                    {
                        ModelState.AddModelError("UserWorkStation.User.Email", $"This user is already assigned to workstation {conflictingUserWorkStation.WorkStation.WorkStationNumber}.");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserWorkStation.User.Email", "User not found.");
                }
            }
            else
            {
                var existingUserWorkStation = await _context.AspNetUserWorkStations
                    .FirstOrDefaultAsync(uw => uw.WorkStationId == workStation.Id);

                if (existingUserWorkStation != null)
                {
                    _context.AspNetUserWorkStations.Remove(existingUserWorkStation);
                    await _context.SaveChangesAsync();
                    workStation.UserWorkStation = null;
                }
            }

            if (await _context.WorkStations.AnyAsync(ws => ws.WorkStationNumber == workStation.WorkStationNumber && ws.Id != workStation.Id))
            {
                ModelState.AddModelError("WorkStationNumber", "A workstation with this number already exists.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingWorkStation = await _context.WorkStations
                        .Include(ws => ws.UserWorkStation)
                        .FirstOrDefaultAsync(ws => ws.Id == id);

                    if (existingWorkStation == null)
                    {
                        return NotFound();
                    }

                    _context.Entry(existingWorkStation).CurrentValues.SetValues(workStation);

                    if (workStation.UserWorkStation != null)
                    {
                        existingWorkStation.UserWorkStation = new AspNetUserWorkStation
                        {
                            UserId = workStation.UserWorkStation.UserId,
                            WorkStationId = workStation.Id
                        };
                    }
                    else
                    {
                        existingWorkStation.UserWorkStation = null;
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!WorkStationExists(workStation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another user after you got the original value. The edit operation was canceled. If you still want to edit this record, click the Save button again.");
                    }
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    ModelState.AddModelError("WorkStationNumber", "A workstation with this number already exists.");
                }
            }

            var userEmails = await _context.Users
                .Select(u => u.Email)
                .Distinct()
                .OrderBy(email => email)
                .ToListAsync();
            ViewData["UserEmails"] = new SelectList(userEmails, workStation.UserWorkStation?.User?.Email);
            ViewData["PCModels"] = new SelectList(await _context.DeviceModels.Where(dm => dm.Type == "PC").Select(dm => dm.Name).ToListAsync(), workStation.PC);
            ViewData["DisplayModels"] = new SelectList(await _context.DeviceModels.Where(dm => dm.Type == "Display").Select(dm => dm.Name).ToListAsync(), workStation.Display);
            return View(workStation);
        }

        // GET: WorkStations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workStation = await _context.WorkStations
                .Include(ws => ws.UserWorkStation)
                .ThenInclude(uw => uw.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workStation == null)
            {
                return NotFound();
            }

            return View(workStation);
        }

        // POST: WorkStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workStation = await _context.WorkStations.FindAsync(id);
            if (workStation != null)
            {
                var userWorkStation = await _context.AspNetUserWorkStations
                    .FirstOrDefaultAsync(uw => uw.WorkStationId == workStation.Id);
                if (userWorkStation != null)
                {
                    _context.AspNetUserWorkStations.Remove(userWorkStation);
                }

                _context.WorkStations.Remove(workStation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool WorkStationExists(int id)
        {
            return _context.WorkStations.Any(e => e.Id == id);
        }
    }
}
