using System.Data.SqlTypes;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qotos.Data;
using Qotos.Models;
using Qotos.Models.Authentication;

namespace Qotos.Controllers
{
	public class AccountsController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _context;


		public AccountsController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
		{
			_logger = logger;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_context = context;
		}

		[Authorize]
		public IActionResult Index()
		{
			return Ok(_signInManager.IsSignedIn(User));
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginModel loginModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(loginModel.Email);
				if (user == null)
				{
					var result = await _signInManager.PasswordSignInAsync(loginModel.Email,
																					loginModel.Password, loginModel.RememberMe, lockoutOnFailure: true);
					if (result.Succeeded)
					{
						return Redirect("/");
					}
					else
					{
						ModelState.AddModelError("Email", "*Tài khoản hoặc mật khẩu không đúng.");
						return View();
					}
				}
				else
				{
					var result = await _signInManager.PasswordSignInAsync(user.UserName,
													loginModel.Password, loginModel.RememberMe, lockoutOnFailure: true);
					if (result.Succeeded)
					{
						return Redirect("/");
					}
					else
					{
						ModelState.AddModelError("Email", "*Tài khoản hoặc mật khẩu không đúng.");
						return View();
					}
				}

			}
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel registerModel)
		{
			if (ModelState.IsValid)
			{
				var userExists = await _userManager.FindByEmailAsync(registerModel.Email);
				if (userExists != null)
				{
					ModelState.AddModelError("Email", "*Email đã tồn tại.");
					return View();
				}

				var time = (DateTime.Now - new DateTime(1970, 1, 1));
				long timeSpan = (long)(time.TotalMilliseconds);

				AppUser user = new AppUser()
				{
					FirstName = registerModel.FirstName,
					LastName = registerModel.LastName,
					Email = registerModel.Email,
					UserName = registerModel.FirstName.ToLower() + registerModel.LastName.ToLower() + timeSpan,
					EmailConfirmed = true,
					SecurityStamp = Guid.NewGuid().ToString(),
				};


				var result = await _userManager.CreateAsync(user, registerModel.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("Login");
				}
				else
				{
					ModelState.AddModelError("Email", "*Tạo tài khoản không thành công.");

				}
			}
			return View();
		}
		public IActionResult ForgotPassword()
		{
			return View();

		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Redirect("/");
		}

		[Authorize]
		public IActionResult Info()
		{
			var user = _userManager.GetUserAsync(User).Result;
			return View(user);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Info(UserUpdateModel userUpdate)
		{
			var user = _userManager.GetUserAsync(User).Result;
			ModelState.Remove("Thumbnail");
			if (ModelState.IsValid)
			{
				if (userUpdate.File != null)
				{
					using (var memoryStream = new MemoryStream())
					{
						await userUpdate.File.CopyToAsync(memoryStream);
						if (memoryStream.Length > 0.5 * 1024 * 1024)
						{
							ModelState.AddModelError("Thumbnail", "*Ảnh phải có kích thước nhỏ hơn 0.5MB.");
							return View(user);
						}
						byte[] bytes = memoryStream.ToArray();
						user.Thumbnail = "data:image/webp;base64," + Convert.ToBase64String(bytes);
					}
				}
				user.FirstName = userUpdate.FirstName;
				user.LastName = userUpdate.LastName;
				user.Email = userUpdate.Email;
				user.UserName = userUpdate.UserName;
				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					return RedirectToAction("Info");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						if (error.Code == "InvalidUserName")
						{
							ModelState.AddModelError("UserName", "*Tên đăng nhập không chứa các ký tự đặc biệt.");
						}
						else if (error.Code == "DuplicateUserName")
						{
							ModelState.AddModelError("UserName", "*Tên đăng nhập đã tồn tại.");
						}
						else if (error.Code == "DuplicateEmail")
						{
							ModelState.AddModelError("Email", "*Địa chỉ email đã tồn tại.");
						}
						else
						{
							return Ok(result.Errors);
						}
					}
				}
			}
			return View(user);
		}

		[Authorize]
		public async Task<IActionResult> Profile()
		{
			var user = _userManager.GetUserAsync(User).Result;
			string url = $"/accounts/@{user.UserName}/profile";
			return Redirect(url);
		}
		// Hồ sơ cá nhân
		[Route("{controller}/@{username}/{action}")]
		public async Task<IActionResult> Profile(string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			var photos = _context.Photos?
			.Where(x => x.UserId == user.Id)
			.Include(x => x.User)
			.Include(x => x.Likes)
			.ToList();
			ViewData["Data"] = new
			{
				PhotosCount = photos?.Count(),
				LikesCount = _context.Likes?.Where(x => x.UserId == user.Id).ToList().Count(),
				CollectionsCount = _context.Collections?.Where(x => x.UserId == user.Id).ToList().Count(),
				AccountUserName = user.UserName,
				AccountDescription = user.Description,
				AccountThumbnail = user.Thumbnail,
				AccountId = user.Id,
			};

			return View("Profile", photos);
		}

		[Route("{controller}/@{username}/{action}")]
		public async Task<IActionResult> Like(string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			var likedPhotos = _context.Likes?
				.Where(x => x.UserId == user.Id)
				.Include(p => p.User)
				.Include(l => l.Photo)
				.ToList();

			ViewData["Data"] = new
			{
				PhotosCount = _context.Photos?.Where(x => x.UserId == user.Id).ToList().Count(),
				LikesCount = likedPhotos?.Count(),
				CollectionsCount = _context.Collections?.Where(x => x.UserId == user.Id).ToList().Count(),
				AccountUserName = user.UserName,
				AccountDescription = user.Description,
				AccountThumbnail = user.Thumbnail,
				AccountId = user.Id,
			};
			return View("Like", likedPhotos);
		}


		[Route("{controller}/@{username}/{action}")]
		public async Task<IActionResult> Collection(string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			var collections = _context.Collections?
				.Where(x => x.UserId == user.Id)
				.Include(x => x.PhotoCollections)
				.ThenInclude(x => x.Photo)
				.ToList();
			ViewData["Data"] = new
			{
				PhotosCount = _context.Photos?.Where(x => x.UserId == user.Id).ToList().Count(),
				LikesCount = _context.Likes?.Where(x => x.UserId == user.Id).ToList().Count(),
				CollectionsCount = collections?.Count(),
				AccountUserName = user.UserName,
				AccountDescription = user.Description,
				AccountThumbnail = user.Thumbnail,
				AccountId = user.Id,
			};
			return View("Collection", collections);
		}

		[Authorize]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
		{
			var user = _userManager.GetUserAsync(User).Result;
			if (ModelState.IsValid)
			{
				var result = await _userManager.ChangePasswordAsync(user, changePasswordModel.Password, changePasswordModel.NewPassword);
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Info));
				}
				else
				{
					if (result.Errors.Any())
					{
						foreach (var item in result.Errors)
						{
							if (item.Code == "PasswordMismatch")
							{
								ModelState.AddModelError("Password", "*Mật khẩu không đúng.");
								return View();
							}
						}
					}
				}
			}
			return View();
		}


		[Authorize]
		public IActionResult History()
		{
			var userId = _userManager.GetUserId(User);
			var photos = _context.UserDownloads?
			.Include(x => x.Photo).ThenInclude(x => x.User)
			.Include(x => x.Photo).ThenInclude(x => x.Likes)
			.Where(x => x.UserId == userId).Select(x => x.Photo)
			.ToList();
			return View(photos);
		}

		[Authorize]
		public IActionResult HistoryDelete()
		{
			var userId = _userManager.GetUserId(User);
			var userDownloads = _context.UserDownloads.Where(x => x.UserId == userId).ToList();
			_context.UserDownloads.RemoveRange(userDownloads);
			_context.SaveChanges();
			return Ok("Success");
		}

		[Authorize]
		public IActionResult Delete()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Delete(UserDeleteModel userDeleteModel)
		{
			if (ModelState.IsValid)
			{
				var user = _userManager.GetUserAsync(User).Result;
				if (user.UserName != userDeleteModel.UserName)
				{
					ModelState.AddModelError("UserName", "*Tên tài khoản không trùng khớp.");
					return View();
				}
				// Action Delete Tài khoản
				// -----Xoá Photo và Collection liên quan
				var collections = _context.Collections?.Where(x => x.UserId == user.Id);
				if (collections != null)
				{
					foreach (var item in collections)
					{
						var collectionPhotos = item.PhotoCollections;
						if (collectionPhotos != null)
						{
							_context.PhotoCollections?.RemoveRange(collectionPhotos);
						}
					}
				}
				// -----Xoá các ảnh đã like
				var likes = _context.Likes?.Where(x => x.UserId == user.Id);
				if (likes != null)
				{
					_context.RemoveRange(likes);
				}
				// -----Xoá các ảnh đã lưu
				var photos = _context.Photos?.Where(x => x.UserId == user.Id).ToList();
				if (photos != null)
				{
					foreach (var item in photos)
					{
						DeletePhotoById(item.Id);
					}
				}
				// -----Xoá tài khoản
				await _userManager.DeleteAsync(user);
				await _context.SaveChangesAsync();
				await _signInManager.SignOutAsync();
				return Redirect("/");
			}
			return View();
		}


		public string DeletePhotoById(string id)
		{
			var user = _userManager.GetUserAsync(User).Result;
			if (user == null)
			{
				return "Unauthorized";
			}

			var photo = _context.Photos?.FirstOrDefault(x => x.Id == id);

			if (photo == null)
			{
				return "NotFound";
			}

			if (photo.UserId != user.Id)
			{
				return "Forbid";
			}

			// Xoá các Likes
			var likes = _context.Likes?.Where(x => x.PhotoId == photo.Id).ToList();
			_context.Likes?.RemoveRange(likes);

			// Xoá các collection liên quan
			var photoCollections = _context.PhotoCollections?.Where(x => x.PhotoId == photo.Id).ToList();
			_context.PhotoCollections?.RemoveRange(photoCollections);

			// Xoá tác PhotoTag
			var photoTags = _context.PhotoTags?.Where(x => x.PhotoId == photo.Id).ToList();
			_context.PhotoTags?.RemoveRange(photoTags);

			// Xoá các Download
			var userDownloads = _context.UserDownloads?.Where(x => x.PhotoId == photo.Id).ToList();
			_context.UserDownloads?.RemoveRange(userDownloads);

			_context.Photos?.Remove(photo);

			return "Success";
		}
	}
}
