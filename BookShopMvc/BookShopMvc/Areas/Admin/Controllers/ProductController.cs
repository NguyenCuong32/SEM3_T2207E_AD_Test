using System;
using BookShopMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Authorization;

namespace BookShopMvc.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
    public class ProductController:Controller
	{
		private readonly BookDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(BookDbContext dbContext, IWebHostEnvironment webHostEnvironment)
		{
			_dbContext = dbContext;
			_webHostEnvironment = webHostEnvironment;
		}

        public IActionResult Index()
        {
            var product = _dbContext.Products.Include(p => p.Author).Include(p => p.Subcategory).Include(p => p.Publisher).OrderByDescending(p => p.Id).ToList();
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(product, settings);

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "json");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, "products.json");

            System.IO.File.WriteAllText(filePath, json);

            return View(product);
        }

		[HttpGet]
		public IActionResult Add()
		{
            ViewBag.Authors = new SelectList(_dbContext.Authors, "Id", "Name");
            ViewBag.SubCategories = new SelectList(_dbContext.Subcategories, "Id", "Name");
            ViewBag.Publishers = new SelectList(_dbContext.Publishers, "Id", "Name");
            return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product, IFormFile imageUpload)
        {
            ModelState.Remove("Image");
            ModelState.Remove("ImageUpload");
            ModelState.Remove("Carts");
            ModelState.Remove("Thumb");
            ModelState.Remove("Author");
            ModelState.Remove("Reviews");
            ModelState.Remove("Publisher");
            ModelState.Remove("Order_items");
            ModelState.Remove("Subcategory");
            ViewBag.Authors = new SelectList(_dbContext.Authors, "Id", "Name");
            ViewBag.SubCategories = new SelectList(_dbContext.Subcategories, "Id", "Name");
            ViewBag.Publishers = new SelectList(_dbContext.Publishers, "Id", "Name");
            if (ModelState.IsValid)
            {
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "backend/images/Product");
                    string ImageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, ImageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Thumb = ImageName;
                }
                _dbContext.Add(product);
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
            return View(product);
        }

        public IActionResult Fix(int Id)
        {
            ViewBag.Authors = new SelectList(_dbContext.Authors, "Id", "Name");
            ViewBag.SubCategories = new SelectList(_dbContext.Subcategories, "Id", "Name");
            ViewBag.Publishers = new SelectList(_dbContext.Publishers, "Id", "Name");
            var product = _dbContext.Products.Find(Id);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fix(Product product, int Id, IFormFile imageUpload)
        {
            ModelState.Remove("Image");
            ModelState.Remove("ImageUpload");
            ModelState.Remove("Carts");
            ModelState.Remove("Thumb");
            ModelState.Remove("Author");
            ModelState.Remove("Reviews");
            ModelState.Remove("Publisher");
            ModelState.Remove("Order_items");
            ModelState.Remove("Subcategory");
            ViewBag.Authors = new SelectList(_dbContext.Authors, "Id", "Name");
            ViewBag.Categories = new SelectList(_dbContext.Subcategories, "Id", "Name");
            ViewBag.PublishingCompanies = new SelectList(_dbContext.Publishers, "Id", "Name");

            if (ModelState.IsValid)
            {
                var existingProduct = await _dbContext.Products.FindAsync(Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                if (imageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "backend/images/Product");
                    string imageName = Guid.NewGuid().ToString() + "_" + imageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await imageUpload.CopyToAsync(fs);
                    }
                    existingProduct.Thumb = imageName;
                }

                // Cập nhật các trường khác của đối tượng sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Slug = product.Slug;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Qty = product.Qty;
                existingProduct.AuthorId = product.AuthorId;
                existingProduct.SubcategoryId = product.SubcategoryId;
                existingProduct.PublisherId = product.PublisherId;
                existingProduct.Status = product.Status;

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
            return View(product);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            Product product = await _dbContext.Products.FindAsync(Id);
            if (!string.Equals(product.Thumb, "noname.jpg"))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "backend/images/Product");
                string filePath = Path.Combine(uploadsDir, product.Thumb);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}

