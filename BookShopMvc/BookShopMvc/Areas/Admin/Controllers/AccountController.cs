using System;
using BookShopMvc.Models;
using BookShopMvc.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShopMvc.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AccountController:Controller
	{
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private readonly BookDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, BookDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;

        }
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            ModelState.Remove("Img");
            ModelState.Remove("Carts");
            ModelState.Remove("Phone");
            ModelState.Remove("Orders");
            ModelState.Remove("Address");
            ModelState.Remove("Reviews");
            ModelState.Remove("Fullname");
            ModelState.Remove("ReturnUrl");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginVM.Username);

                if (user != null && await _userManager.CheckPasswordAsync(user, loginVM.Password))
                {
                    var result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);

                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        if (roles.Contains("Admin") || roles.Contains("VipAdmin"))
                        {
                            return Redirect(loginVM.ReturnUrl ?? "/Admin");
                        }
                        else
                        {
                            // Hiển thị thông báo lỗi hoặc chuyển hướng đến trang không có quyền truy cập
                            ModelState.AddModelError("", "You are not authorized to access this site");
                            return View(loginVM);
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid Email or password");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                var errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(loginVM);
        }

        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}

