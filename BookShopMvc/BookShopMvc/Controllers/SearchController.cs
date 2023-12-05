using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopMvc.Controllers
{
	public class SearchController:Controller
	{
		private readonly BookDbContext _dbContext;
		public SearchController(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IActionResult> Index(string search)
		{
            var product = await _dbContext.Products.Include(p => p.Subcategory).Include(p => p.Author).Where(p => p.Name.Contains(search)).ToListAsync();
            ViewBag.SearchQuery = search;
            return View(product);
		}
	}
}

