using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopMvc.Repository.Components
{
	public class HomeProductViewComponent:ViewComponent	{
		private readonly BookDbContext _dbContext;
		public HomeProductViewComponent(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _dbContext.Categories
                .Include(c => c.Subcategories).ThenInclude(s => s.Products).ThenInclude(a => a.Author).Where(c => c.Status == 1)
                .ToListAsync();

            return View(categories);
        }
    }
}

