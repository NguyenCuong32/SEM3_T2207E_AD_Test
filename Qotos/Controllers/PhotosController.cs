using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qotos.Data;
using Qotos.Models;
using Qotos.Models.Authentication;

namespace Qotos.Controllers
{
	public class PhotosController : Controller
	{

		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;
		public PhotosController(ApplicationDbContext context, IMapper mapper, UserManager<AppUser> userManager)
		{
			_context = context;
			_mapper = mapper;
			_userManager = userManager;
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Submit(Photo photo)
		{
			if (photo.FileImg != null)
			{
				photo.Extension = System.IO.Path.GetExtension(photo.FileImg.FileName);
				using (var memoryStream = new MemoryStream())
				{
					await photo.FileImg.CopyToAsync(memoryStream);
					byte[] bytes = memoryStream.ToArray();
					photo.Thumbnail = "data:image/webp;base64," + Convert.ToBase64String(bytes);
				}
			}
			var userId = _userManager.GetUserId(User);
			photo.UserId = userId;
			photo.PhotoTags = new List<PhotoTag>();

			// Thêm tags
			if (photo.Tags != null)
			{
				foreach (var tag in photo.Tags)
				{
					var existingTag = _context.Tags?.FirstOrDefault(c => c.TagName == tag);
					if (existingTag == null)
					{
						existingTag = new Tag { TagName = tag };
						_context.Tags?.Add(existingTag);
					}
					photo.PhotoTags.Add(new PhotoTag { Tag = existingTag });
				}
			}
			_context.Photos?.Add(photo);
			await _context.SaveChangesAsync();
			return Redirect("/accounts/profile");
		}

		[Authorize]
		public IActionResult Like(string id)
		{
			var user = _userManager.GetUserAsync(User).Result;
			var photo = _context.Photos?.FirstOrDefault(x => x.Id == id);
			if (photo == null)
			{
				return NotFound();
			}
			var existingLike = _context.Likes?.FirstOrDefault(l => l.UserId == user.Id && l.PhotoId == photo.Id);
			if (existingLike != null)
			{
				_context.Likes?.Remove(existingLike);
				_context.SaveChanges();
				return Ok("Cancel like");
			}
			else
			{
				var newLike = new Like
				{
					UserId = user.Id,
					PhotoId = photo.Id
				};
				_context.Likes?.Add(newLike);
				_context.SaveChanges();
				return Ok("Like");
			}

		}

		public IActionResult View(string id)
		{
			var photo = _context.Photos?
			.Include(x => x.User)
			.Include(x => x.Likes)
			.Include(x => x.PhotoTags)
			.ThenInclude(x => x.Tag)
			.FirstOrDefault(x => x.Id == id);
			if (photo == null)
			{
				return NotFound();
			}
			photo.Views++;
			_context.Photos?.Update(photo);
			_context.SaveChanges();
			return Ok(photo);
		}

		public IActionResult Download(string id)
		{
			var photo = _context.Photos?.Include(x => x.User).FirstOrDefault(x => x.Id == id);
			if (photo == null)
			{
				return NotFound();
			}
			var user = _userManager.GetUserAsync(User).Result;
			if (user != null)
			{
				var userDownloadExists = _context.UserDownloads?.Where(x => x.UserId == user.Id && x.PhotoId == photo.Id).FirstOrDefault();
				if (userDownloadExists == null)
				{
					var userDownload = new UserDownload
					{
						UserId = user.Id,
						PhotoId = photo.Id
					};
					_context.UserDownloads?.Add(userDownload);
				}
			}
			photo.Downloads++;
			_context.Photos?.Update(photo);
			_context.SaveChanges();
			return Ok(photo);
		}

		public IActionResult Tag(string id)
		{
			// Increase count search tag
			var tag = _context.Tags?.FirstOrDefault(x => x.TagName == id);
			if (tag != null)
			{
				tag.Searchs++;
			}
			_context.SaveChanges();

			var tags = _context.Tags?
				.Where(x => x.TagName.Contains(id)).Select(x => x.TagName)
				.ToList();
			ViewData["Tags"] = tags;

			var photos = _context.Photos?
					.Where(p => p.PhotoTags.Any(pt => pt.Tag.TagName.Contains(id)))
					.Include(x => x.User)
					.Include(x => x.Likes)
					.ToList();
			ViewData["Key"] = id;
			return View(photos);
		}


		// [Authorize]
		[HttpPost]
		public IActionResult Edit(string id, [FromBody] Photo data)
		{
			var user = _userManager.GetUserAsync(User).Result;
			var photo = _context.Photos?.Include(x => x.PhotoTags).FirstOrDefault(x => x.Id == id);
			if (photo == null)
			{
				return NotFound();
			}
			if (photo.UserId != user.Id)
			{
				return BadRequest();
			}
			photo.Description = data.Description;
			photo.Camera = data.Camera;
			photo.Location = data.Location;
			//Update Tags

			// Cập nhật Tags
			if (data.Tags != null && data.Tags.Any())
			{
				_context.PhotoTags?.RemoveRange(photo.PhotoTags);
				foreach (var tag in data.Tags)
				{
					var existingTag = _context.Tags?.FirstOrDefault(t => t.TagName == tag);
					if (existingTag == null)
					{
						existingTag = new Tag { TagName = tag };
						_context.Tags?.Add(existingTag);
					}
					photo.PhotoTags?.Add(new PhotoTag { Tag = existingTag });
				}
			}
			_context.Photos?.Update(photo);
			_context.SaveChanges();
			return Ok("Success");
		}

		[Authorize]
		[HttpDelete]
		public async Task<IActionResult> Delete(string id)
		{
			var user = _userManager.GetUserAsync(User).Result;
			if (user == null)
			{
				return Unauthorized();
			}

			var photo = _context.Photos?.FirstOrDefault(x => x.Id == id);

			if (photo == null)
			{
				return NotFound();
			}

			if (photo.UserId != user.Id)
			{
				return Forbid();
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
			await _context.SaveChangesAsync();

			return Ok("Success");
		}


		[Authorize]
		public IActionResult Report(string id)
		{
			var user = _userManager.GetUserAsync(User).Result;
			var photo = _context.Photos?.FirstOrDefault(x => x.Id == id);
			if (photo == null)
			{
				return NotFound();
			}

			var rp = _context.Reports.FirstOrDefault(x => x.UserId == user.Id && x.PhotoId == photo.Id);
			if (rp != null)
			{
				return Ok("Exists");
			}
			else
			{
				Report newRp = new Report()
				{
					PhotoId = photo.Id,
					UserId = user.Id
				};
				_context.Reports?.Add(newRp);
				_context.SaveChanges();
				return Ok("Success");
			}

		}

	}
}
