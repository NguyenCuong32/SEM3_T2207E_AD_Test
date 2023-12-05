using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopMvc.Controllers
{
	public class ProductController:Controller
	{
		private readonly BookDbContext _dbContext;
		public ProductController(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IActionResult> Index(int Id)
		{
			var product = _dbContext.Products.Include(p => p.Subcategory).Include(p => p.Author).Include(p => p.Reviews).Where(p => p.Id == Id);

			return View(product);
		}

	}
}

