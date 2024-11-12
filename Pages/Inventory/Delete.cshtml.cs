using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Pages.Inventory
{
    public class DeleteModel : PageModel
    {
        private readonly WebApp.Data.ApplicationDbContext _context;

        public DeleteModel(WebApp.Data.ApplicationDbContext context)
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

            if (inventoryitem == null)
            {
                return NotFound();
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
                _context.InventoryItem.Remove(InventoryItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
