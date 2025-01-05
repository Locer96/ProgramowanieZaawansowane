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
using Microsoft.AspNetCore.Identity;

namespace InventoryApp.Pages.Users
{
	[Authorize(Policy = "RequireAdministratorRole")]
	public class DeleteModel : PageModel
	{
		private readonly InventoryApp.Data.ApplicationDbContext _context;

		public DeleteModel(InventoryApp.Data.ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public IdentityUser User { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

			if (user == null)
			{
				return NotFound();
			}
			else
			{
				User = user;
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.Users.FindAsync(id);
			if (user != null)
			{
				User = user;
				_context.Users.Remove(User);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}
