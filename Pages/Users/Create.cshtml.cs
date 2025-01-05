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
using Microsoft.AspNetCore.Identity;

namespace InventoryApp.Pages.Users
{
	[Authorize(Policy = "RequireAdministratorRole")]
	public class CreateModel : PageModel
	{
		private readonly InventoryApp.Data.ApplicationDbContext _context;

		public CreateModel(InventoryApp.Data.ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public IdentityUser User { get; set; } = default!;

		public IActionResult OnGet()
		{
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_context.Users.Add(User);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}
