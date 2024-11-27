using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventoryApp.Data;
using InventoryApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace InventoryApp.Pages.Inventory
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly InventoryApp.Data.ApplicationDbContext _context;

        public CreateModel(InventoryApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public InventoryItem InventoryItem { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InventoryItem.Add(InventoryItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
