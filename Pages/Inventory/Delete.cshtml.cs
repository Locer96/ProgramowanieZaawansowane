using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryApp.Data;
using InventoryApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace InventoryApp.Pages.Inventory
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly InventoryApp.Data.ApplicationDbContext _context;

        public DeleteModel(InventoryApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InventoryItem InventoryItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryitem = await _context.InventoryItem.FirstOrDefaultAsync(m => m.Id == id);

            if (inventoryitem == null || (inventoryitem.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Administrator")))
            {
                return Forbid();
            }
            else
            {
                InventoryItem = inventoryitem;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryitem = await _context.InventoryItem.FindAsync(id);
            if (inventoryitem != null)
            {
                InventoryItem = inventoryitem;
                if (InventoryItem.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Administrator"))
                {
                    return Forbid();
                }
                _context.InventoryItem.Remove(InventoryItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
