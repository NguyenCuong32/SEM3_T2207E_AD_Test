using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Qotos.Data;
using Qotos.Models;
using Qotos.Models.Authentication;

namespace Qotos.Controllers
{
	public class CollectionsController : Controller
	{

		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;
		public CollectionsController(ApplicationDbContext context, IMapper mapper, UserManager<AppUser> userManager)
		{
			_context = context;
			_mapper = mapper;
			_userManager = userManager;
		}


		[Authorize]
		public IActionResult Index()
		{
			var user = _userManager.GetUserAsync(User).Result;
			var collections = _context.Collections?
				.Include(x => x.PhotoCollections)
				.Where(x => x.UserId == user.Id)
				.ToList();
			if (collections == null)
			{
				return NotFound();
			}
			return Ok(collections);
		}


		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] Collection data, string id)
		{
			var user = await _userManager.GetUserAsync(User);
			var title = data.Title;
			var collectionExists = _context.Collections?.FirstOrDefault(x => x.Title == title && x.UserId == user.Id);
			if (collectionExists != null)
			{
				return Ok("Exists");
			}

			var photo = _context.Photos?.FirstOrDefault(p => p.Id == id);

			if (photo == null)
			{
				return NotFound("Photo not found");
			}

			Collection collection = new Collection
			{
				Title = title,
				UserId = user.Id,
				PhotoCollections = new List<PhotoCollection>()
			};

			// Tạo mới PhotoColection
			var newPhotoCollection = new PhotoCollection
			{
				PhotoId = id
			};

			collection.PhotoCollections.Add(newPhotoCollection);

			_context.Collections?.Add(collection);
			await _context.SaveChangesAsync();
			return Ok(collection.Id);
		}


		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Add(string id, string photoId)
		{
			string result = "decrease";
			var user = await _userManager.GetUserAsync(User);
			var collection = _context.Collections?.Include(x => x.PhotoCollections).FirstOrDefault(x => x.Id == id && x.UserId == user.Id);
			if (collection == null)
			{
				return NotFound();
			}

			var collectionPhoto = collection.PhotoCollections;
			if(collectionPhoto != null) { }

			var existingPhotoCollection = collectionPhoto?.FirstOrDefault(pc => pc.PhotoId == photoId);

			if (existingPhotoCollection != null)
			{
				collection.PhotoCollections?.Remove(existingPhotoCollection);
			}
			else
			{
				var newPhotoCollection = new PhotoCollection
				{
					PhotoId = photoId
				};
				collection.PhotoCollections?.Add(newPhotoCollection);
				result = "increase";
			}
			await _context.SaveChangesAsync();
			return Ok(result);
		}

		public IActionResult Details(string id)
		{
			var collection = _context.Collections?
				.Include(x => x.PhotoCollections)
				.ThenInclude(x => x.Photo)
				.ThenInclude(x => x.User)
				.Include(x => x.PhotoCollections)
				.ThenInclude(x => x.Photo)
				.ThenInclude(x => x.Likes)
				.Include(x => x.User)
				.FirstOrDefault(x => x.Id == id);
			return View(collection);
		}


		[Authorize]
		[HttpGet("api/collections/details/{id}")]
		public IActionResult ApiDetails(string id)
		{
			var collection = _context.Collections?
				.FirstOrDefault(x => x.Id == id);
			return Ok(collection);
		}


		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Update([FromBody] Collection data, string id)
		{
			var user = await _userManager.GetUserAsync(User);
			var collection = _context.Collections?.FirstOrDefault(x => x.Id == id);
			if (collection == null)
			{
				return NotFound();
			}
			if (user.Id != collection.UserId)
			{
				return BadRequest();
			}
			collection.Title = data.Title;
			_context.Collections?.Update(collection);
			await _context.SaveChangesAsync();
			return Ok("Update successful");
		}

		[Authorize]
		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] Collection data, string id)
		{
			var user = await _userManager.GetUserAsync(User);
			var collection = _context.Collections?
			.Include(x => x.PhotoCollections)
			.ThenInclude(x => x.Photo)
			.FirstOrDefault(x => x.Id == id);

			if (collection == null)
			{
				return NotFound();
			}
			if (user.Id != collection.UserId)
			{
				return BadRequest();
			}

			if(collection.PhotoCollections != null)
			{
				_context.PhotoCollections?.RemoveRange(collection.PhotoCollections);
			}

			_context.Collections?.Remove(collection);

			await _context.SaveChangesAsync();
			return Ok("Delete successful");
		}

	}
}
