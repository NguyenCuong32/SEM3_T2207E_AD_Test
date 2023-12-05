using System;
using BookShopMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopMvc.Controllers
{
	public class FilterController:Controller
	{
		private readonly BookDbContext _dbContext;
		public FilterController(BookDbContext dbContext)
		{
			_dbContext = dbContext;
		}
        public async Task<IActionResult> FilterProducts(string pricemin, string pricemax, string author, int publisher)
        {
            if (string.IsNullOrEmpty(pricemin) && string.IsNullOrEmpty(pricemax) && string.IsNullOrEmpty(author) && publisher == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                IQueryable<Product> query = _dbContext.Products.Include(p => p.Subcategory).Include(p => p.Author).Include(p => p.Publisher);
                // Lưu giá trị của các tham số lọc
                ViewBag.PriceMin = pricemin;
                ViewBag.PriceMax = pricemax;
                ViewBag.Author = author;
                ViewBag.Publisher = publisher;
                if (!string.IsNullOrEmpty(pricemin))
                {
                    decimal minPrice;
                    if (decimal.TryParse(pricemin, out minPrice))
                    {
                        query = query.Where(p => p.Price >= minPrice);
                    }
                }
                if (!string.IsNullOrEmpty(pricemax))
                {
                    decimal maxPrice;
                    if (decimal.TryParse(pricemax, out maxPrice))
                    {
                        query = query.Where(p => p.Price <= maxPrice);
                    }
                }
                // Lọc sản phẩm dựa trên tác giả (author)
                if (!string.IsNullOrEmpty(author))
                {
                    query = query.Where(p => p.Author.Name == author);
                }

                // Lọc sản phẩm dựa trên publisher
                if (publisher != 0)
                {
                    query = query.Where(p => p.Publisher.Id == publisher);
                }
                var products = await query.ToListAsync();
                return View(products);
            }
        }

    }
}

