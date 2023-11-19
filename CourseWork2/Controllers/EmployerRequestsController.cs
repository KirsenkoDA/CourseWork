using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork2.Data;
using EmploymentAgency.Models;
using Microsoft.AspNetCore.Identity;
using CourseWork2.Models;
using NuGet.Versioning;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;

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
        //Отклик на резюме
        [Authorize]
        public async Task<IActionResult> Respond(int? id)
        {
            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            Account account = new Account();
            account = await _context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.User == identityUser);
            if (id == null || _context.EmployerRequest == null)
            {
                return NotFound();
            }

            var EmployerRequest = await _context.EmployerRequest.Include(a => a.Responds).FirstOrDefaultAsync(m => m.Id == id);
            if (EmployerRequest == null)
            {
                return NotFound();
            }
            EmployerRequest.Responds.Add(account);
            _context.Update(EmployerRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Публикация резюме модератором
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> Publish(int? id)
        {
            if (id == null || _context.EmployerRequest == null)
            {
                return NotFound();
            }

            var EmployerRequest = await _context.EmployerRequest.FindAsync(id);
            if (EmployerRequest == null)
            {
                return NotFound();
            }
            EmployerRequest.Status = _context.Statuses.Find(2);
            _context.Update(EmployerRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: EmployerRequest
        [Authorize]
        public async Task<IActionResult> Index(int Id)
        {
            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (Id == 1)
            {
                //Для соискателя
                //Фильтр по текущему пользователю "Мои резюме"
                var applicationDbContextFiltered = _context.EmployerRequest
                    .Include(e => e.Education)
                    .Include(e => e.Status)
                    .Include(e => e.User)
                    .Where(e => e.UserId == identityUser.Id).ToList();
                ViewData["filteredValues"] = Id;
                return View(applicationDbContextFiltered);
            }
            else
            {
                //Для работодателя
                //Выборка всех значений со статусом "Опубликовано"
                var applicationDbContext = _context.EmployerRequest
                    .Include(e => e.Education)
                    .Include(e => e.Status)
                    .Include(e => e.User)
                    .Where(e => e.StatusId == 2)
                    .ToList();
                return View(applicationDbContext);
            }
        }

        // GET: EmployerRequest/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployerRequest == null)
            {
                return NotFound();
            }

            var EmployerRequest = await _context.EmployerRequest
                .Include(e => e.Education)
                .Include(e => e.Status)
                .Include(e => e.User)
                .Include(e => e.Responds).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (EmployerRequest == null)
            {
                return NotFound();
            }
            return View(EmployerRequest);
        }
        // GET: EmployerRequest/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: EmployerRequest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,DateCreated,Post,Info,EducationId,Salary,StatusId")] EmployerRequest EmployerRequest)
        {
            //if (ModelState.IsValid)
            //{
            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            //Account account = new Account();
            //account = await _context.Accounts
            //    .Include(a => a.EmployerRequest)
            //    .FirstOrDefaultAsync(m => m.User == identityUser);
            EmployerRequest.DateCreated = DateTime.Now;
            EmployerRequest.Status = _context.Statuses.Find(1);
            EmployerRequest.User = _userManager.GetUserAsync(HttpContext.User).Result;
            EmployerRequest.Education = _context.Educations.Find(EmployerRequest.EducationId);
            //account.EmployerRequest.Add(EmployerRequest);
            //_context.Update(account);
            _context.Add(EmployerRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = 1 });
            //}
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Name", EmployerRequest.EducationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", EmployerRequest.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", EmployerRequest.UserId);
            return View(EmployerRequest);
        }

        // GET: EmployerRequest/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployerRequest == null)
            {
                return NotFound();
            }

            var EmployerRequest = await _context.EmployerRequest.FindAsync(id);
            if (EmployerRequest == null)
            {
                return NotFound();
            }
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Name", EmployerRequest.EducationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", EmployerRequest.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", EmployerRequest.UserId);
            return View(EmployerRequest);
        }

        // POST: EmployerRequest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,DateCreated,Post,Info,EducationId,Salary,StatusId")] EmployerRequest EmployerRequest)
        {
            if (id != EmployerRequest.Id)
            {
                return NotFound();
            }

            try
            {
                var existingEmployerRequest = await _context.EmployerRequest
                    .Include(a => a.Responds)
                    .FirstOrDefaultAsync(m => m.Id == EmployerRequest.Id);
                existingEmployerRequest.Status = _context.Statuses.Find(1);
                existingEmployerRequest.Post = EmployerRequest.Post;
                existingEmployerRequest.Info = EmployerRequest.Info;
                existingEmployerRequest.EducationId = EmployerRequest.EducationId;
                existingEmployerRequest.Salary = EmployerRequest.Salary;
                _context.Update(existingEmployerRequest);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployerRequestExists(EmployerRequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index", new { id = 1 });
        }

        // GET: EmployerRequest/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployerRequest == null)
            {
                return NotFound();
            }

            var EmployerRequest = await _context.EmployerRequest
                .Include(r => r.Education)
                .Include(r => r.Status)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (EmployerRequest == null)
            {
                return NotFound();
            }

            return View(EmployerRequest);
        }

        // POST: EmployerRequest/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployerRequest == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EmployerRequest'  is null.");
            }
            var EmployerRequest = await _context.EmployerRequest.FindAsync(id);
            if (EmployerRequest != null)
            {
                _context.EmployerRequest.Remove(EmployerRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployerRequestExists(int id)
        {
            return (_context.EmployerRequest?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
