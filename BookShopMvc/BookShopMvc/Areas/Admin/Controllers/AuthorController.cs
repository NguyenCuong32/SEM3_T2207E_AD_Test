using System;
using BookShopMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopMvc.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
    public class AuthorController:Controller
	{
		private readonly BookDbContext _dbContext;
		public AuthorController(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public IActionResult Index()
		{
            var author = _dbContext.Authors.OrderByDescending(a => a.Id).ToList();
			return View(author);
		}
		public IActionResult Add()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(Author author)
		{
            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                _dbContext.Add(author);
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
            return View(author);
        }

        public IActionResult Fix (int Id)
        {
            var author = _dbContext.Authors.Find(Id);
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fix (Author author)
        {
            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                _dbContext.Update(author);
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
            return View(author);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            Author author = await _dbContext.Authors.FindAsync(Id);
            _dbContext.Authors.Remove(author);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
	}
}

