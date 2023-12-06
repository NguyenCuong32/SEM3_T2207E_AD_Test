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
	public class TagsController : Controller
	{

		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;
		public TagsController(ApplicationDbContext context, IMapper mapper, UserManager<AppUser> userManager)
		{
			_context = context;
			_mapper = mapper;
			_userManager = userManager;
		}

		public IActionResult Search(string id, string key)
		{
			var tags = _context.Tags?
								.Where(x => x.TagName.Contains(id)).ToList();
			// Kiểm tra nếu key không có trong param
			switch (key)
			{
				case "hot":
					tags = _context.Tags?
								.OrderByDescending(x => x.Searchs)
								.Take(10).ToList();
					break;
				case "all":
					tags = _context.Tags?.ToList();
					break;
				default:
					break;
			}
			return Ok(tags);
		}

	}
}
