using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopMvc.Controllers
{
	public class SubCategoryController:Controller
	{
		private readonly BookDbContext _dbContext;
		public SubCategoryController(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IActionResult> Index(string Slug)
		{
			var SubCategory = await _dbContext.Subcategories.Include(s => s.Products).ThenInclude(a => a.Author).Where(s => s.Slug == Slug).ToListAsync();
			return View(SubCategory);
		}
	}
}

