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

        public IList<InventoryItem> InventoryItem { get;set; } = default!;

        public async Task OnGetAsync()
        {
            InventoryItem = await _context.InventoryItem.ToListAsync();
        }
    }
}
