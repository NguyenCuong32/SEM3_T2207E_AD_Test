using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qotos.Data;
using Qotos.Models;
using System.Diagnostics;

namespace Qotos.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;


		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			var photos = _context.Photos?
			.Include(x => x.User)
			.Include(x => x.Likes)
			.OrderByDescending(x => x.Publish)
			.Take(12).ToList();
			return View(photos);
		}


		// GET/home/?page=2
		public IActionResult Page(int id)
		{
			var count = _context.Photos?.Count();
			if (count < (id - 1) * 12)
			{
				return Ok("end");
			}
			var photos = _context.Photos
				.Include(x => x.User)
				.Include(x => x.Likes)
				.Skip(12 * (id - 1)).Take(12)
				.OrderByDescending(x => x.Publish)
				.ToList();
			return Ok(photos);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}