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
using System.Security.Claims;

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
            var emptyInventoryItem = new InventoryItem();

            if (await TryUpdateModelAsync<InventoryApp.Models.InventoryItem>(emptyInventoryItem, "inventoryitem",
                item => item.UpdateDate,
                item => item.PC,
                item => item.Display,
                item => item.Keyboard,
                item => item.Mouse))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                emptyInventoryItem.UserId = userId;
                _context.InventoryItem.Add(emptyInventoryItem);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
