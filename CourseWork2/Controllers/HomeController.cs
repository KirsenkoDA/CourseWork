using CourseWork2.Data;
using CourseWork2.Models;
using EmploymentAgency.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CourseWork2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        //Стрраница соискателя
        [HttpGet("ApplicantHome")]
        public async Task<IActionResult> ApplicantHome()
        {
            return View();
        }
        //Страница работодателя
        [HttpGet("EmployerHome")]
        public async Task<IActionResult> EmployerHome()
        {
            return View();
        }
        //[HttpGet("IndexForApplicant")]
        //public async Task<IActionResult> IndexForApplicant()
        //{
        //    var applicationDbContext = _context.EmployerRequests.Include(e => e.Education);
        //    return View(await applicationDbContext.ToListAsync());
        //}
        //[HttpGet("IndexForEmployer")]
        //public async Task<IActionResult> IndexForEmployer()
        //{
        //    List<Resume> publishedResumes = new List<Resume>();
        //        publishedResumes.Add(await _context.Resumes
        //        .Include(r => r.Education)
        //        .Include(r => r.Status)
        //        .Include(r => r.User)
        //        .FirstOrDefaultAsync(m => m.Status == _context.Statuses.FirstOrDefault(n => n.Id == 2)));

        //    return View(publishedResumes);
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}