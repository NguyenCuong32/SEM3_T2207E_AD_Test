using System;
using System.Data;
using BookShopMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShopMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController:Controller
	{
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private readonly BookDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, BookDbContext dbContext, RoleManager<IdentityRole> roleManager)
		{
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }
		public IActionResult Index()
		{
            var role = _roleManager.Roles.ToList();
            return View(role);
		}

		public IActionResult Add()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(IdentityRole addRole)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = addRole.Name
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                // Xử lý khi tạo vai trò không thành công
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(addRole);
        }

        public IActionResult Fix(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            if(role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fix(IdentityRole addRole)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                var result = _roleManager.UpdateAsync(addRole).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                // Xử lý khi tạo vai trò không thành công
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(addRole);
        }

        public async Task<IActionResult> Delete(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if(role == null)
            {
                return NotFound();
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Xử lý khi xoá vai trò không thành công
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Error"); // Hiển thị view Error hoặc xử lý theo ý muốn của bạn
            }
        }

    }
}

