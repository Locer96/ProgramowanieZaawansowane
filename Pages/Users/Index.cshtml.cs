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
        public int TotalUsers { get; set; }

        public string IdSort { get; set; }
        public string EmailSort { get; set; }
        public string EmailConfirmedSort { get; set; }
        public string PhoneNumberSort { get; set; }
        public string PhoneNumberConfirmedSort { get; set; }
        public string AdminSort { get; set; }
        public string CurrentSort { get; set; }
        public Dictionary<string, bool> IsAdmin { get; set; } = new Dictionary<string, bool>();

        public async Task OnGetAsync(string sortOrder)
        {
            IdSort = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            EmailSort = sortOrder == "Email" ? "email_desc" : "Email";
            EmailConfirmedSort = sortOrder == "EmailConfirmed" ? "emailConfirmed_desc" : "EmailConfirmed";
            PhoneNumberSort = sortOrder == "PhoneNumber" ? "phoneNumber_desc" : "PhoneNumber";
            PhoneNumberConfirmedSort = sortOrder == "PhoneNumberConfirmed" ? "phoneNumberConfirmed_desc" : "PhoneNumberConfirmed";
            AdminSort = sortOrder == "Admin" ? "admin_desc" : "Admin";
            CurrentSort = sortOrder;

            var users = from u in _context.Users select u;

            switch (sortOrder)
            {
                case "id_desc":
                    users = users.OrderByDescending(u => u.Id);
                    break;
                case "Email":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                case "EmailConfirmed":
                    users = users.OrderBy(u => u.EmailConfirmed);
                    break;
                case "emailConfirmed_desc":
                    users = users.OrderByDescending(u => u.EmailConfirmed);
                    break;
                case "PhoneNumber":
                    users = users.OrderBy(u => u.PhoneNumber);
                    break;
                case "phoneNumber_desc":
                    users = users.OrderByDescending(u => u.PhoneNumber);
                    break;
                case "PhoneNumberConfirmed":
                    users = users.OrderBy(u => u.PhoneNumberConfirmed);
                    break;
                case "phoneNumberConfirmed_desc":
                    users = users.OrderByDescending(u => u.PhoneNumberConfirmed);
                    break;
                case "Admin":
                    users = users.OrderBy(u => IsAdmin[u.Id]);
                    break;
                case "admin_desc":
                    users = users.OrderByDescending(u => IsAdmin[u.Id]);
                    break;
                default:
                    users = users.OrderBy(u => u.Id);
                    break;
            }

            Users = await users.AsNoTracking().ToListAsync();
            TotalUsers = Users.Count;

            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Administrator");
            var adminUserIds = new List<string>();

            if (adminRole != null)
            {
                adminUserIds = await _context.UserRoles
                    .Where(ur => ur.RoleId == adminRole.Id)
                    .Select(ur => ur.UserId)
                    .ToListAsync();
            }

            foreach (var user in Users)
            {
                IsAdmin[user.Id] = adminUserIds.Contains(user.Id);
            }
        }
    }
}
