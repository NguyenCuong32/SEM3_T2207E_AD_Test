using System;
using BookShopMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookShopMvc.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
    public class SubcategoryController:Controller
	{
		private readonly BookDbContext _dbContext;
		public SubcategoryController(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public IActionResult Index()
		{
			var sub = _dbContext.Subcategories.Include(s => s.Category).OrderByDescending(s => s.Id).ToList();
			return View(sub);
		}
		public IActionResult Add()
		{
            ViewBag.SubCategory = new SelectList(_dbContext.Categories, "Id", "Name");
            return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(Subcategory subcategory)
		{
            var sub = await _dbContext.Categories.ToListAsync();
            ViewBag.SubCategory = new SelectList(sub, "Id", "Name", subcategory.CategoryId);
			ModelState.Remove("Category");
			ModelState.Remove("Products");
            if (ModelState.IsValid)
			{
				_dbContext.Add(subcategory);
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
            return View(subcategory);
        }

		public IActionResult Fix(int Id)
		{
           ViewBag.SubCategory = new SelectList(_dbContext.Categories, "Id", "Name");
		   var sub = _dbContext.Subcategories.Find(Id);
		   return View(sub);

        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult>Fix(Subcategory subcategory)
		{
            var sub = await _dbContext.Categories.ToListAsync();
            ViewBag.SubCategory = new SelectList(sub, "Id", "Name", subcategory.CategoryId);
            ModelState.Remove("Category");
            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                _dbContext.Update(subcategory);
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
            return View(subcategory);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            Subcategory sub = await _dbContext.Subcategories.FindAsync(Id);
            _dbContext.Subcategories.Remove(sub);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

