using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryApp.Data;
using InventoryApp.Models;

namespace InventoryApp.Pages.Inventory
{
    public class DetailsModel : PageModel
    {
        private readonly InventoryApp.Data.ApplicationDbContext _context;

        public DetailsModel(InventoryApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
