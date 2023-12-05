using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;
using WebApplication1.entity;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly FashionContext _context;

        public AccountController(FashionContext context)
        {
            _context = context;
        }

        // GET: Account

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'FashionContext.Users'  is null.");
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Username == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Password,Email,RewardPoint")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
                userModel.Password = hashedPassword;
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,Password,Email,RewardPoint")] UserModel userModel)
        {
            if (id != userModel.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserModelExists(userModel.Username))
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
            return View(userModel);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Username == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'FashionContext.Users'  is null.");
            }
            var userModel = await _context.Users.FindAsync(id);
            if (userModel != null)
            {
                _context.Users.Remove(userModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserModelExists(string id)
        {
            return (_context.Users?.Any(e => e.Username == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<IActionResult> OnPostLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostLogin(string username, string password, string ReturnUrl)
        {
            UserModel u = _context.Users.FirstOrDefault(x => x.Username == username);
            if (u != null && BCrypt.Net.BCrypt.Verify(password, u.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim("User", JsonSerializer.Serialize(u)),
                };
                var identity = new ClaimsIdentity(claims, "User");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                });

                _context.SaveChanges();
                return ReturnUrl == null ? Redirect("../Home") : Redirect(ReturnUrl);
            }
            else
            {
                ViewBag.Message = "Tài khoản hoặc mật khẩu không chính xác";
                return View();

            }
        }

        [HttpGet]
        public async Task<IActionResult> register()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(OnPostLogin));
        }



        [HttpPost]
        public async Task<IActionResult> register(string username, string password, string confirmPassword, string email)
        {

                if (string.IsNullOrEmpty(username))
                {
                    ViewBag.Message = "Vui lòng nhập username";
                    return View();
                }
                else if (string.IsNullOrEmpty(email))
                {
                    ViewBag.Message = "Vui lòng nhập email";
                    return View();

                }
                else if (string.IsNullOrEmpty(password))
                {
                    ViewBag.Message = "Vui lòng nhập password";
                    return View();

                }
                else if (string.IsNullOrEmpty(confirmPassword))
                {
                    ViewBag.Message = "Vui lòng nhập lại mật khẩu.";
                    return View();

                }
                else if (password.Equals(confirmPassword) == false)
                {
                    ViewBag.Message = "Mật khẩu và xác nhận mật khẩu không trùng khớp.";
                    return View();
                }
                UserModel userModel = new UserModel();
                userModel.Username = username;
                userModel.Email = email;
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                userModel.Password = hashedPassword;
                userModel.RewardPoint = 0;
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(OnPostLogin));

        }



}
}
