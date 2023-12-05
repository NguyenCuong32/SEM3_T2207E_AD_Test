using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopMvc.Repository.Components
{
	public class SidebarViewComponent:ViewComponent
	{
		private readonly BookDbContext _dbContext;
		public SidebarViewComponent(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Subcategories = await _dbContext.Categories.Include(c => c.Subcategories).ToListAsync();
            Subcategories.ForEach(category =>
            {
                category.Subcategories = category.Subcategories.ToList();
            });
            return View(Subcategories);
        }
    }
}

