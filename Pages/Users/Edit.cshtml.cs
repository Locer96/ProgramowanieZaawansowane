using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryApp.Data;
using InventoryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace InventoryApp.Pages.Users
{
	[Authorize(Policy = "RequireAdministratorRole")]
	public class EditModel : PageModel
	{
		private readonly InventoryApp.Data.ApplicationDbContext _context;

		public EditModel(InventoryApp.Data.ApplicationDbContext context)
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
			User = user;
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == User.Id);
			if (userInDb == null)
			{
				return NotFound();
			}

			userInDb.UserName = User.UserName;
			userInDb.Email = User.Email;
            userInDb.EmailConfirmed = User.EmailConfirmed;
            userInDb.PhoneNumber = User.PhoneNumber;
            userInDb.PhoneNumberConfirmed = User.PhoneNumberConfirmed;

            try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(User.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToPage("./Index");
		}

		private bool UserExists(string id)
		{
			return _context.Users.Any(e => e.Id == id);
		}
	}
}
