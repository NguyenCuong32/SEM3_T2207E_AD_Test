using System;
using System.Security.Claims;
using BookShopMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShopMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AddAccountController:Controller
	{
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private readonly BookDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AddAccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, BookDbContext dbContext, RoleManager<IdentityRole> roleManager)
		{
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

		public IActionResult Index()
		{
            var adminUsers = _userManager.GetUsersInRoleAsync("Admin").Result;
            var nonUserAdmins = adminUsers.Where(u => !_userManager.IsInRoleAsync(u, "User").Result).ToList();
            return View(nonUserAdmins);
        }

        public IActionResult Add()
        {
           
            var roles = _roleManager.Roles.ToList(); // Lấy danh sách vai trò từ RoleManager

            var viewModel = new Users
            {
                Roles = roles
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Users users)
        {
            ModelState.Remove("Img");
            ModelState.Remove("Carts");
            ModelState.Remove("Phone");
            ModelState.Remove("Orders");
            ModelState.Remove("Address");
            ModelState.Remove("Reviews");
            ModelState.Remove("Fullname");
            ModelState.Remove("Roles");
            if (ModelState.IsValid)
            {
                // Tạo một instance mới của AppUser với thông tin từ model
                var user = new AppUser
                {
                    UserName = users.Username,
                    Email = users.Email
                };

                // Tạo người dùng với password đã được hash
                var result = await _userManager.CreateAsync(user, users.Password);

                if (result.Succeeded)
                {
                    // Kiểm tra xem RoleId được chọn có hợp lệ hay không
                    if (!string.IsNullOrEmpty(users.RoleId))
                    {
                        // Tìm vai trò dựa trên RoleId
                        var role = await _roleManager.FindByIdAsync(users.RoleId);

                        if (role != null)
                        {
                            // Thêm người dùng vào vai trò
                            await _userManager.AddToRoleAsync(user, role.Name);
                        }
                    }

                    // Đăng nhập người dùng sau khi đã tạo thành công
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Chuyển hướng đến trang thành công hoặc thực hiện các thao tác khác
                    return RedirectToAction("Index");
                }

                // Xử lý lỗi nếu có
                foreach (var error in result.Errors)
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

            // Nếu ModelState không hợp lệ, hiển thị lại form với thông tin đã nhập và thông báo lỗi
            users.Roles = _roleManager.Roles.ToList(); // Lấy danh sách vai trò
            return View(users);
        }

    }
}

