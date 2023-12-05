using System;
using BookShopMvc.Models;
using BookShopMvc.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShopMvc.Controllers
{
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
			return View(new LoginViewModel { ReturnUrl = returnUrl});
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

                        if (roles.Contains("User"))
                        {
                            return Redirect(loginVM.ReturnUrl ?? "/");
                        }
                        else
                        {
                            // Hiển thị thông báo lỗi hoặc chuyển hướng đến trang không có quyền truy cập
                            ModelState.AddModelError("", "This is the user page");
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
        public IActionResult Create()
		{
			return View();
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Users user)
        {
            ModelState.Remove("Img");
            ModelState.Remove("Carts");
            ModelState.Remove("Phone");
            ModelState.Remove("Orders");
            ModelState.Remove("Address");
            ModelState.Remove("Reviews");
            ModelState.Remove("Fullname");
            ModelState.Remove("RoleId");
            ModelState.Remove("Roles");
            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser { UserName = user.Username, Email = user.Email };
                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    // Tạo vai trò người dùng nếu chưa tồn tại
                    var roleExists = await _roleManager.RoleExistsAsync("User");
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    // Gán vai trò "User" cho người dùng
                    await _userManager.AddToRoleAsync(newUser, "User");

                    // Cập nhật ConcurrencyStamp của người dùng mới
                    //newUser.ConcurrencyStamp = Guid.NewGuid().ToString();
                    //await _userManager.UpdateAsync(newUser);
                    user.Password = newUser.PasswordHash;
                    _dbContext.Add(user);
                    await _dbContext.SaveChangesAsync();
                    return Redirect("/Account/Login");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
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

            return View(user);
        }

        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

    }
}

