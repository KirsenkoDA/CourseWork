﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork2.Data;
using EmploymentAgency.Models;

namespace CourseWork2.Controllers
{
    public class Resumes1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public Resumes1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Resumes1
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Resumes.Include(r => r.Education).Include(r => r.Status).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Resumes1/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Resumes1/Create
        public IActionResult Create()
        {
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Resumes1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,DateCreated,Post,Info,EducationId,Salary,StatusId")] Resume resume)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resume);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EducationId"] = new SelectList(_context.Educations, "Id", "Id", resume.EducationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", resume.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", resume.UserId);
            return View(resume);
        }

        // GET: Resumes1/Edit/5
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

        // POST: Resumes1/Edit/5
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

        // GET: Resumes1/Delete/5
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

        // POST: Resumes1/Delete/5
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