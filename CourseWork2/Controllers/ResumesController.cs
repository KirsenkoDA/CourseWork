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
    public class ResumesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public ResumesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
            if (id == null || _context.Resumes == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes.Include(a => a.Responds).FirstOrDefaultAsync(m => m.Id == id);
            if (resume == null)
            {
                return NotFound();
            }
            resume.Responds.Add(account);
            _context.Update(resume);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Публикация резюме модератором
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> Publish(int? id)
        {
            if (id == null || _context.Resumes == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes.FindAsync(id);
            if (resume == null)
            {
                return NotFound();
            }
            resume.Status = _context.Statuses.Find(2);
            _context.Update(resume);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Resumes
        [Authorize]
        public async Task<IActionResult> Index(int Id)
        {
            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (Id == 1)
            {
                //Для соискателя
                //Фильтр по текущему пользователю "Мои резюме"
                var applicationDbContextFiltered = _context.Resumes
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
                var applicationDbContext = _context.Resumes
                    .Include(e => e.Education)
                    .Include(e => e.Status)
                    .Include(e => e.User)
                    .Where(e => e.StatusId == 2)
                    .ToList();
                return View(applicationDbContext);
            }
        }

        // GET: Resumes/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Resumes == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .Include(e => e.Education)
                .Include(e => e.Status)
                .Include(e => e.User)
                .Include(e => e.Responds).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }
        // GET: Resumes/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Resumes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,DateCreated,Post,Info,EducationId,Salary,StatusId")] Resume resume)
        {
            //if (ModelState.IsValid)
            //{
            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            //Account account = new Account();
            //account = await _context.Accounts
            //    .Include(a => a.Resumes)
            //    .FirstOrDefaultAsync(m => m.User == identityUser);
            resume.DateCreated = DateTime.Now;
            resume.Status = _context.Statuses.Find(1);
            resume.User = _userManager.GetUserAsync(HttpContext.User).Result;
            resume.Education = _context.Educations.Find(resume.EducationId);
            //account.Resumes.Add(resume);
            //_context.Update(account);
            _context.Add(resume);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = 1 });
            //}
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Name", resume.EducationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", resume.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", resume.UserId);
            return View(resume);
        }

        // GET: Resumes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Resumes == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes.FindAsync(id);
            if (resume == null)
            {
                return NotFound();
            }
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Name", resume.EducationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", resume.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", resume.UserId);
            return View(resume);
        }

        // POST: Resumes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,DateCreated,Post,Info,EducationId,Salary,StatusId")] Resume resume)
        {
            if (id != resume.Id)
            {
                return NotFound();
            }

            try
            {
                var existingResume = await _context.Resumes
                    .Include(a => a.Responds)
                    .FirstOrDefaultAsync(m => m.Id == resume.Id);
                existingResume.Status = _context.Statuses.Find(1);
                existingResume.Post = resume.Post;
                existingResume.Info = resume.Info;
                existingResume.EducationId = resume.EducationId;
                existingResume.Salary = resume.Salary;
                _context.Update(existingResume);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResumeExists(resume.Id))
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

        // GET: Resumes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Resumes == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .Include(r => r.Education)
                .Include(r => r.Status)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // POST: Resumes/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Resumes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Resumes'  is null.");
            }
            var resume = await _context.Resumes.FindAsync(id);
            if (resume != null)
            {
                _context.Resumes.Remove(resume);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResumeExists(int id)
        {
          return (_context.Resumes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
