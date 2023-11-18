using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CourseWork2.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseWork2.Controllers
{
    public class UsersController : Controller
    {
        UserManager<IdentityUser> _userManager;
        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult Index() => View(_userManager.Users.ToList());
        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult Create() => View();
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Edit(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}