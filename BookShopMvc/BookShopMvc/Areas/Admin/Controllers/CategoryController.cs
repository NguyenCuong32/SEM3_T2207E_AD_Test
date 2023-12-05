using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookShopMvc;
using BookShopMvc.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookShopMvc.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
    public class CategoryController:Controller
	{
        private readonly BookDbContext _dbContext;
        public CategoryController(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public IActionResult Index()
		{
			var category = _dbContext.Categories.OrderByDescending(c => c.Id).ToList();
			return View(category);
		}
		public IActionResult Add()
		{
			return View();
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Category category)
        {
            ModelState.Remove("Subcategories");
            if (ModelState.IsValid)
            {
                _dbContext.Add(category);
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
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Fix(int Id)
		{
			var category = _dbContext.Categories.Find(Id);
			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Fix(int Id, Category category)
		{
            ModelState.Remove("Subcategories");
            if (ModelState.IsValid)
            {
                _dbContext.Update(category);
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
            return View(category);
        }

        public async Task<IActionResult>Delete(int Id)
        {
            Category cate = await _dbContext.Categories.FindAsync(Id);
            _dbContext.Categories.Remove(cate);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
	}
}

