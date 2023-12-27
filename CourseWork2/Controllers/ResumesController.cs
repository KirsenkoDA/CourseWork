using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.EntityFrameworkCore;
using CourseWork2.Data;
using EmploymentAgency.Models;
using Microsoft.AspNetCore.Identity;
using CourseWork2.Models;
using NuGet.Versioning;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using NLog;
using X.PagedList;

namespace CourseWork2.Controllers
{
    public class ResumesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly ILogger<ResumesController> _logger;

        public ResumesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<ResumesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        // GET: Resumes
        [Authorize]
        public async Task<IActionResult> Index(int Id, int? pageNumber, string? searchText)
        {
            const int pageSize = 3;
            int pageCount;

            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            Account userAccount = _context.Accounts.FirstOrDefault(u => u.User.Id == identityUser.Id);
            Task<IPagedList<Resume>> applicationDbContext;

            if (User.IsInRole("MODERATOR"))
            {
                applicationDbContext = _context.Resumes
                    .Include(e => e.Education)
                    .Include(e => e.Status)
                    .Include(e => e.User)
                    .Include(e => e.Responds)
                        .ThenInclude(u => u.User)
                    .ToPagedListAsync(pageNumber ?? 1, pageSize);

                pageCount = (int)Math.Ceiling(_context.Resumes.Count() / (float)pageSize);

                ViewData["pageCount"] = pageCount;
                return View(await applicationDbContext);
            }
            else
            {
                if (Id == 1)
                {
                    //Для соискателя
                    //Фильтр по текущему пользователю "Мои резюме"
                    if (searchText != null)
                    {
                        //Фильтр поиска по части названия
                        applicationDbContext = _context.Resumes
                        .Include(e => e.Education)
                        .Include(e => e.Status)
                        .Include(e => e.User)
                        .Include(e => e.Responds)
                            .ThenInclude(u => u.User)
                        .Where(e => (e.UserId == identityUser.Id) && (e.Post.Contains(searchText)))
                        .ToPagedListAsync(pageNumber ?? 1, pageSize);
                        pageCount = (int)Math.Ceiling(_context.Resumes.Where(e => e.UserId == identityUser.Id && (e.Post.Contains(searchText))).Count() / (float)pageSize);
                    }
                    else
                    {
                        //Без фильтра поиска по части названия
                        applicationDbContext = _context.Resumes
                            .Include(e => e.Education)
                            .Include(e => e.Status)
                            .Include(e => e.User)
                            .Include(e => e.Responds)
                                .ThenInclude(u => u.User)
                            .Where(e => e.UserId == identityUser.Id)
                            .ToPagedListAsync(pageNumber ?? 1, pageSize);
                        pageCount = (int)Math.Ceiling(_context.Resumes.Where(e => e.UserId == identityUser.Id).Count() / (float)pageSize);
                    }
                    ViewData["filteredValues"] = Id;
                    ViewData["pageCount"] = pageCount;
                    return View(await applicationDbContext);
                }
                else
                {
                    //Для работодателя
                    //Выборка всех значений со статусом "Опубликовано"
                    if (searchText != null)
                    {
                        //Фильтр поиска по части названия
                        applicationDbContext = _context.Resumes
                        .Include(e => e.Education)
                        .Include(e => e.Status)
                        .Include(e => e.User)
                        .Include(e => e.Responds)
                            .ThenInclude(u => u.User)
                        .Where(e => (e.StatusId == 2) && (e.Post.Contains(searchText)))
                        .ToPagedListAsync(pageNumber ?? 1, pageSize);
                        pageCount = (int)Math.Ceiling(_context.Resumes.Where(e => e.StatusId == 2 && (e.Post.Contains(searchText))).Count() / (float)pageSize);
                    }
                    else
                    {
                        //Без фильтра поиска по части названия
                        applicationDbContext = _context.Resumes
                            .Include(e => e.Education)
                            .Include(e => e.Status)
                            .Include(e => e.User)
                            .Include(e => e.Responds)
                                .ThenInclude(u => u.User)
                            .Where(e => e.StatusId == 2)
                            .ToPagedListAsync(pageNumber ?? 1, pageSize);
                        pageCount = (int)Math.Ceiling(_context.Resumes.Where(e => e.StatusId == 2).Count() / (float)pageSize);
                    }

                    if (userAccount == null)
                    {
                        _logger.LogInformation("Для того чтобы откликнуться на резюме, создайте аккаунт и перезайтие в свою учётную запись");
                        ViewBag.Message = "Для того чтобы откликнуться на резюме, создайте аккаунт и перезайтие в свою учётную запись";
                    }
                    ViewData["searchText"] = searchText;
                    ViewData["pageCount"] = pageCount;
                    return View(await applicationDbContext);
                }
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
                .Include(e => e.Responds)
                    .ThenInclude(r => r.User)
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
            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            resume.DateCreated = DateTime.Now;
            resume.Status = _context.Statuses.Find(1);
            resume.User = _userManager.GetUserAsync(HttpContext.User).Result;
            resume.Education = _context.Educations.Find(resume.EducationId);
            _context.Add(resume);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = 1 });
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
            return RedirectToAction("Index", new { id = 1 });
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
