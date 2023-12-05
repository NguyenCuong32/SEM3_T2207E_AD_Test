using System;
using BookShopMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopMvc.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
    public class PublisherController:Controller
	{
		private readonly BookDbContext _dbContext;
		public PublisherController(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public IActionResult Index()
		{
			var publisher = _dbContext.Publishers.OrderByDescending(p => p.Id).ToList();
			return View(publisher);
		}
		public IActionResult Add()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(Publisher publisher)
		{
			ModelState.Remove("Products");
			if (ModelState.IsValid)
			{
				_dbContext.Add(publisher);
				await _dbContext.SaveChangesAsync();
				return RedirectToAction("Index");
			}
            else
            {
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(publisher);
        }

		public IActionResult Fix(int Id)
		{
			var publisher = _dbContext.Publishers.Find(Id);
			return View(publisher);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Fix(int Id, Publisher publisher)
		{
            ModelState.Remove("Products");
            if (ModelState.IsValid)
			{
				_dbContext.Update(publisher);
				await _dbContext.SaveChangesAsync();
				return RedirectToAction("Index");
			}
            else
            {
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(publisher);
        }

		public async Task<IActionResult> Delete(int Id)
		{
			Publisher publisher = await _dbContext.Publishers.FindAsync(Id);
			_dbContext.Publishers.Remove(publisher);
			await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
	}
}

