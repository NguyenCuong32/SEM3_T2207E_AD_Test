using Microsoft.AspNetCore.Mvc;
using Qotos.Data;


namespace QShop.Components.Articles
{
	public class Tags : ViewComponent
	{

		private readonly ApplicationDbContext _context;
		public Tags(ApplicationDbContext context)
		{
			_context = context;
		}

		public IViewComponentResult Invoke()
		{
			var tags = _context.Tags?.ToList();
			return View(tags);
		}
	}
}
