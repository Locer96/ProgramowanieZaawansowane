using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryApp.Data;
using InventoryApp.Models;

namespace InventoryApp.Controllers
{
    [Authorize]
    public class MyWorkspaceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MyWorkspaceController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MyWorkspace
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var userWorkStation = await _context.AspNetUserWorkStations
                .Include(uw => uw.WorkStation)
                .FirstOrDefaultAsync(uw => uw.UserId == user.Id);

            if (userWorkStation == null)
            {
                return View(null);
            }

            return View(userWorkStation.WorkStation);
        }
    }
}
