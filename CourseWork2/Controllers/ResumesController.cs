﻿using System;
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

namespace CourseWork2.Controllers
{
    public class ResumesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResumesController(ApplicationDbContext context)
        {
            _context = context;
        }
        //Публикация резюме модератором
        public async Task<IActionResult> Publish(int? id)
        {
            if (id == 1)
            {
                var applicationDbContextFiltered = _context.Resumes
                .Include(e => e.Education)
                .Include(e => e.Status)
                .Include(e => e.User)
                .Where(e => e.StatusId == 2);
                return View(await applicationDbContextFiltered.ToListAsync());
            }
            else
            {
                return NotFound();
            }
            resume.Status = _context.Statuses.Find(2);
            _context.Update(resume);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Resumes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Resumes
                .Include(r => r.Education)
                .Include(r => r.Status)
                .Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }
        [HttpGet("IndexForEmployer")]
        public async Task<IActionResult> IndexForEmployer()
        {
            var applicationDbContext = _context.Resumes
                .Include(r => r.Education)
                .Include(r => r.Status)
                .Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Resumes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Resumes == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .Include(r => r.Education)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // GET: Resumes/Create
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Post,Info,EducationId,Salary")] Resume resume)
        {
            resume.DateCreated = DateTime.Now;
            resume.Status = _context.Statuses.Find(1);
            resume.User = _userManager.GetUserAsync(HttpContext.User).Result;
            resume.Education = _context.Educations.Find(resume.EducationId);
            _context.Add(resume);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Name", resume.EducationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", resume.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", resume.UserId);
            return View(resume);
        }

        // GET: Resumes/Edit/5
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
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Id", resume.EducationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", resume.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", resume.UserId);
            return View(resume);
        }

        // POST: Resumes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,DateCreated,Post,Info,EducationId,Salary,StatusId")] Resume resume)
        {
            if (id != resume.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resume);
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
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Id", resume.EducationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", resume.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", resume.UserId);
            return View(resume);
        }

        // GET: Resumes/Delete/5
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
