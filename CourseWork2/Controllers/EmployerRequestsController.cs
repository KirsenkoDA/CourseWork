﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork2.Data;
using CourseWork2.Models;
using EmploymentAgency.Models;
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
        //Отклик на вакансию
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

            var employerRequest = await _context.EmployerRequest.Include(a => a.Responds).FirstOrDefaultAsync(m => m.Id == id);
            if (employerRequest == null)
            {
                return NotFound();
            }
            employerRequest.Responds.Add(account);
            _context.Update(employerRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Публикация резюме модератором
        public async Task<IActionResult> Publish(int? id)
        {
            if (id == null || _context.EmployerRequest == null)
            {
                return NotFound();
            }

            var employerRequest = await _context.EmployerRequest.FindAsync(id);
            if (employerRequest == null)
            {
                return NotFound();
            }
            employerRequest.Status = _context.Statuses.Find(2);
            _context.Update(employerRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: EmployerRequestNews
        public async Task<IActionResult> Index(int id)
        {
            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (id == 1)
            {
                var applicationDbContextFiltered = _context.EmployerRequest
                .Include(e => e.Education)
                .Include(e => e.Status)
                .Include(e => e.User)
                .Where(e => e.UserId == identityUser.Id)
                .ToList();
                return View(applicationDbContextFiltered);
            }
            else
            {
                var applicationDbContext = _context.EmployerRequest
                .Include(e => e.Education)
                .Include(e => e.Status)
                .Include(e => e.User)
                .Where(e => e.StatusId == 2)
                .ToList();
                return View(applicationDbContext);
            }
        }

        // GET: EmployerRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployerRequests == null)
            {
                return NotFound();
            }

            var employerRequest = await _context.EmployerRequests
                .Include(e => e.Education)
                .Include(e => e.Status)
                .Include(e => e.User)
                .Include(e => e.Responds).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employerRequest == null)
            {
                return NotFound();
            }

            return View(employerRequest);
        }

        // GET: EmployerRequests/Create
        public IActionResult Create()
        {
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Id");
            return View();
        }

        // POST: EmployerRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,DateCreated,Post,Info,EducationId,Salary,StatusId")] EmployerRequest employerRequest)
        {
            //if (ModelState.IsValid)
            //{
            IdentityUser identityUser = _userManager.GetUserAsync(HttpContext.User).Result;
            Account account = new Account();
            account = await _context.Accounts
                .Include(a => a.EmployerRequests)
                .FirstOrDefaultAsync(m => m.User == identityUser);
            employerRequest.DateCreated = DateTime.Now;
            employerRequest.Status = _context.Statuses.Find(1);
            employerRequest.User = identityUser;
            employerRequest.Education = _context.Educations.Find(employerRequest.EducationId);
            account.EmployerRequests.Add(employerRequest);
            _context.Update(account);
            _context.Add(employerRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //}
            //ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Name", employerRequestNew.EducationId);
            //ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", employerRequestNew.StatusId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", employerRequestNew.UserId);
            //return View(employerRequestNew);
        }

        // GET: EmployerRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployerRequests == null)
            {
                return NotFound();
            }

            var employerRequest = await _context.EmployerRequests.FindAsync(id);
            if (employerRequest == null)
            {
                return NotFound();
            }
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Id", employerRequest.EducationId);
            return View(employerRequest);
        }

        // POST: EmployerRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateCreated,Post,Info,EducationId,Salary")] EmployerRequest employerRequest)
        {
            if (id != employerRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employerRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployerRequestExists(employerRequest.Id))
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
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Id", employerRequest.EducationId);
            return View(employerRequest);
        }

        // GET: EmployerRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployerRequests == null)
            {
                return NotFound();
            }

            var employerRequest = await _context.EmployerRequests
                .Include(e => e.Education)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employerRequest == null)
            {
                return NotFound();
            }

            return View(employerRequest);
        }

        // POST: EmployerRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployerRequests == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EmployerRequests'  is null.");
            }
            var employerRequest = await _context.EmployerRequests.FindAsync(id);
            if (employerRequest != null)
            {
                _context.EmployerRequests.Remove(employerRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployerRequestExists(int id)
        {
          return (_context.EmployerRequests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
