using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryApp.Data;
using InventoryApp.Models;
using System.Security.Claims;

namespace InventoryApp.Pages.Users
{
	[Authorize(Policy = "RequireAdministratorRole")]
	public class IndexModel : PageModel
	{
		private readonly InventoryApp.Data.ApplicationDbContext _context;

		public IndexModel(InventoryApp.Data.ApplicationDbContext context)
		{
			_context = context;
		}

		public IList<IdentityUser> Users { get; set; } = default!;

		public async Task OnGetAsync()
		{
			Users = await _context.Users.ToListAsync();
		}
	}
}
