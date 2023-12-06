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
	public class DataController : Controller
	{

		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;
		public DataController(ApplicationDbContext context, IMapper mapper, UserManager<AppUser> userManager)
		{
			_context = context;
			_mapper = mapper;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			Random rand = new Random();
			var tags = _context.Tags?.ToList();
			for (int i = 0; i < 50; i++)
			{
				List<Tag> tagNews = new List<Tag>() { };

				int stringlen = rand.Next(4, 10);
				int randValue;
				string str = "";
				char letter;
				for (int j = 0; j < stringlen; j++)
				{
					randValue = rand.Next(0, 26);
					letter = Convert.ToChar(randValue + 65);
					str = str + letter;
				}
				Photo photo = new Photo
				{
					Thumbnail = "https://i.pravatar.cc/150?img=" + rand.Next(100),
					UserId = "162c40c0037d",
				};
				_context.Photos?.Add(photo);
				foreach (var item in tags)
				{
					var lucky = rand.Next(100);
					if (lucky % 2 == 0)
					{
						PhotoTag photoTag = new PhotoTag()
						{
							PhotoId = photo.Id,
							TagId = item.Id
						};
						_context.PhotoTags?.Add(photoTag);
					}
				}
				await _context.SaveChangesAsync();
			}
			return Ok("Success full");
		}
	}
}
