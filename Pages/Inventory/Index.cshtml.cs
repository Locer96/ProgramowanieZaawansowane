using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryApp.Data;
using InventoryApp.Models;
using System.Security.Claims;

namespace InventoryApp.Pages.Inventory
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly InventoryApp.Data.ApplicationDbContext _context;

        public IndexModel(InventoryApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InventoryItem> InventoryItem { get; set; } = default!;
        public string UserIdSort { get; set; }
        public string UpdateDateSort { get; set; }
        public string PCSort { get; set; }
        public string DisplaySort { get; set; }
        public string KeyboardSort { get; set; }
        public string MouseSort { get; set; }
        public string CurrentSort { get; set; }
        public int TotalItems { get; set; }
        public int UserItems { get; set; }

        public async Task OnGetAsync(string sortOrder)
        {
            UserIdSort = String.IsNullOrEmpty(sortOrder) ? "userId_desc" : "";
            UpdateDateSort = sortOrder == "UpdateDate" ? "updateDate_desc" : "UpdateDate";
            PCSort = sortOrder == "PC" ? "pc_desc" : "PC";
            DisplaySort = sortOrder == "Display" ? "display_desc" : "Display";
            KeyboardSort = sortOrder == "Keyboard" ? "keyboard_desc" : "Keyboard";
            MouseSort = sortOrder == "Mouse" ? "mouse_desc" : "Mouse";
            CurrentSort = sortOrder;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            TotalItems = await _context.InventoryItem.CountAsync();
            UserItems = await _context.InventoryItem.CountAsync(i => i.UserId == userId);

            IQueryable<InventoryItem> inventoryItemsIQ = from i in _context.InventoryItem
                                                         select i;

            switch (sortOrder)
            {
                case "userId_desc":
                    inventoryItemsIQ = inventoryItemsIQ.OrderByDescending(i => i.UserId);
                    break;
                case "UpdateDate":
                    inventoryItemsIQ = inventoryItemsIQ.OrderBy(i => i.UpdateDate);
                    break;
                case "updateDate_desc":
                    inventoryItemsIQ = inventoryItemsIQ.OrderByDescending(i => i.UpdateDate);
                    break;
                case "PC":
                    inventoryItemsIQ = inventoryItemsIQ.OrderBy(i => i.PC);
                    break;
                case "pc_desc":
                    inventoryItemsIQ = inventoryItemsIQ.OrderByDescending(i => i.PC);
                    break;
                case "Display":
                    inventoryItemsIQ = inventoryItemsIQ.OrderBy(i => i.Display);
                    break;
                case "display_desc":
                    inventoryItemsIQ = inventoryItemsIQ.OrderByDescending(i => i.Display);
                    break;
                case "Keyboard":
                    inventoryItemsIQ = inventoryItemsIQ.OrderBy(i => i.Keyboard);
                    break;
                case "keyboard_desc":
                    inventoryItemsIQ = inventoryItemsIQ.OrderByDescending(i => i.Keyboard);
                    break;
                case "Mouse":
                    inventoryItemsIQ = inventoryItemsIQ.OrderBy(i => i.Mouse);
                    break;
                case "mouse_desc":
                    inventoryItemsIQ = inventoryItemsIQ.OrderByDescending(i => i.Mouse);
                    break;
                default:
                    inventoryItemsIQ = inventoryItemsIQ.OrderBy(i => i.UserId);
                    break;
            }

            InventoryItem = await inventoryItemsIQ.AsNoTracking().ToListAsync();
        }
    }
}
