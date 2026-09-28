using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppUI.Models;
using WebAppUI.Auth;

namespace WebAppUI.Controllers
{
    [Authorize(Roles = AppRoles.Administrator)]
    public class UserManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<UserViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserViewModel { Id = user.Id, Email = user.Email ?? string.Empty, Roles = roles.ToList() });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.AllRoles = AppRoles.All;
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid) { ViewBag.AllRoles = AppRoles.All; return View(model); }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var rolesToAssign = model.SelectedRoles
                    .Where(r => AppRoles.All.Contains(r))
                    .Distinct()
                    .ToList();

                if (rolesToAssign.Count == 0)
                {
                    rolesToAssign.Add(AppRoles.User);
                }

                foreach (var role in rolesToAssign)
                    await _userManager.AddToRoleAsync(user, role);
                TempData["SuccessMessage"] = "Пользователь создан";
                return RedirectToAction("Index");
            }
            ViewBag.AllRoles = AppRoles.All;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            return View(new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                AvailableRoles = AppRoles.All.ToList(),
                SelectedRoles = roles.ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            var rolesToAssign = model.SelectedRoles
                .Where(r => AppRoles.All.Contains(r))
                .Distinct()
                .ToList();

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (rolesToAssign.Count > 0)
            {
                await _userManager.AddToRolesAsync(user, rolesToAssign);
            }

            TempData["SuccessMessage"] = "Роли обновлены";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null) await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            var users = await _userManager.Users
                .Where(u => string.IsNullOrEmpty(term) || (u.Email ?? string.Empty).Contains(term))
                .ToListAsync();

            var model = new List<UserViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserViewModel { Id = user.Id, Email = user.Email ?? string.Empty, Roles = roles.ToList() });
            }
            return PartialView("_UsersTable", model);
        }
    }
}