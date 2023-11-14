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
using EmploymentAgency.Models;

namespace CourseWork2.Controllers
{
    public class EmployerRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public EmployerRequestsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EmployerRequestNews
        public async Task<IActionResult> Index(int id)
        {
            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            Account account = new Account();
            account = await _context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.User == identityUser);
            if (id == null || _context.EmployerRequest == null)
            {
                var applicationDbContextFiltered = _context.EmployerRequest
                .Include(e => e.Education)
                .Include(e => e.Status)
                .Include(e => e.User)
                .Where(e => e.UserId == identityUser.Id);
                return View(await applicationDbContextFiltered.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.EmployerRequest
                .Include(e => e.Education)
                .Include(e => e.Status)
                .Include(e => e.User);
                return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: EmployerRequestNews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployerRequest == null)
            {
                return NotFound();
            }

            var employerRequestNew = await _context.EmployerRequest
                .Include(e => e.Education)
                .Include(e => e.Status)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employerRequestNew == null)
            {
                return NotFound();
            }

            return View(employerRequestNew);
        }

        // GET: EmployerRequestNews/Create
        public IActionResult Create()
        {
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: EmployerRequestNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,DateCreated,Post,Info,EducationId,Salary,StatusId")] EmployerRequest employerRequestNew)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(employerRequestNew);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Name", employerRequestNew.EducationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", employerRequestNew.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", employerRequestNew.UserId);
            return View(employerRequestNew);
        }

        // GET: EmployerRequestNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployerRequest == null)
            {
                return NotFound();
            }

            var employerRequestNew = await _context.EmployerRequest.FindAsync(id);
            if (employerRequestNew == null)
            {
                return NotFound();
            }
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Id", employerRequestNew.EducationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", employerRequestNew.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", employerRequestNew.UserId);
            return View(employerRequestNew);
        }

        // POST: EmployerRequestNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,DateCreated,Post,Info,EducationId,Salary,StatusId")] EmployerRequest employerRequestNew)
        {
            if (id != employerRequestNew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employerRequestNew);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployerRequestNewExists(employerRequestNew.Id))
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
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Id", employerRequestNew.EducationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", employerRequestNew.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", employerRequestNew.UserId);
            return View(employerRequestNew);
        }

        // GET: EmployerRequestNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployerRequest == null)
            {
                return NotFound();
            }

            var employerRequestNew = await _context.EmployerRequest
                .Include(e => e.Education)
                .Include(e => e.Status)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employerRequestNew == null)
            {
                return NotFound();
            }

            return View(employerRequestNew);
        }

        // POST: EmployerRequestNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployerRequest == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EmployerRequestNew'  is null.");
            }
            var employerRequestNew = await _context.EmployerRequest.FindAsync(id);
            if (employerRequestNew != null)
            {
                _context.EmployerRequest.Remove(employerRequestNew);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployerRequestNewExists(int id)
        {
          return (_context.EmployerRequest?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
