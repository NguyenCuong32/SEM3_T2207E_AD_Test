using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopMvc.Repository.Components
{
	public class SidebarDetailViewComponent : ViewComponent
	{
		private readonly BookDbContext _dbContext;
		public SidebarDetailViewComponent(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var publisher = await _dbContext.Publishers.OrderByDescending(p => p.Id).ToListAsync();
			return View(publisher);
		}

    }
}

