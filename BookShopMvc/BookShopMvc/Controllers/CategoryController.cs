using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopMvc.Controllers
{
	public class CategoryController:Controller
	{
		private readonly BookDbContext _dbContext;
		public CategoryController(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IActionResult> Index(string Slug)
		{
			var categories = await _dbContext.Categories
                .Include(c => c.Subcategories)
                .ThenInclude(s => s.Products).ThenInclude(a => a.Author).Where(c => c.Slug == Slug)
                .ToListAsync();
            return View(categories);
		}
	}
}

