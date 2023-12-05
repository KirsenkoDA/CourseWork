using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork2.Data;
using CourseWork2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;

namespace CourseWork2.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        //Аккаунт который видит владелец резме или вакансии
        [Authorize]
        public async Task<IActionResult> AccountPreview(string id)
        {
 
            var applicationDbContextFiltered = _context.Accounts
            .Include(e => e.EmployerRequests)
            .Include(e => e.Resumes)
            .Include(e => e.User)
            .Where(e => e.User.Id == id);
            return View(await applicationDbContextFiltered.ToListAsync());
        }
        // GET: Accounts
        [Authorize]
        public async Task<IActionResult> Index(int id)
        {
            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (id == 1)
            {
                var applicationDbContextFiltered = _context.Accounts
                .Include(e => e.EmployerRequests)
                .Include(e => e.Resumes)
                .Include(e => e.User)
                .Where(e => e.User.Id == identityUser.Id);
                return View(await applicationDbContextFiltered.ToListAsync());
            }
            else
            {
                if (User.IsInRole("MODERATOR"))
                {
                    var applicationDbContextFiltered = _context.Accounts
                    .Include(e => e.EmployerRequests)
                    .Include(e => e.Resumes)
                    .Include(e => e.User);
                    return View(await applicationDbContextFiltered.ToListAsync());
                }
                else
                {
                    var applicationDbContextFiltered = _context.Accounts
                    .Include(e => e.EmployerRequests)
                    .Include(e => e.Resumes)
                    .Include(e => e.User)
                    .Where(e => e.User.Id == identityUser.Id);
                    return View(await applicationDbContextFiltered.ToListAsync());
                }
            }
        }

        // GET: Accounts/Details/5
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        [Authorize]
        public IActionResult Create()
        {
            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var account = _context.Accounts
                .Include(e => e.User)
                .FirstOrDefault(e => e.User.Id == identityUser.Id);
            if(account == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", new { id = 1 });
            }
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Info,Name,PhoneNumber")] Account account)
        {
            account.User = _userManager.GetUserAsync(HttpContext.User).Result;
            _context.Add(account);
            await _context.SaveChangesAsync();

            IdentityUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            await _userManager.AddToRoleAsync(currentUser, "CLIENT");
            AlterUserAsync(account.Name, currentUser);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task AlterUserAsync(string name, IdentityUser currentUser)
        {
            currentUser.UserName = name;
            _context.Update(currentUser);
            await _context.SaveChangesAsync();
        }
        // GET: Accounts/Edit/5
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Info")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
          return (_context.Accounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
