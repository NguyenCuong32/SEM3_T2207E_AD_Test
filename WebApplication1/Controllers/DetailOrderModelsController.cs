using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DetailOrderModelsController : Controller
    {
        private readonly FashionContext _context;

        public DetailOrderModelsController(FashionContext context)
        {
            _context = context;
        }

        // GET: DetailOrderModels
        public async Task<IActionResult> Index()
        {
            var fashionContext = _context.DetailOrders.Include(d => d.Order).Include(d => d.Product);
            return View(await fashionContext.ToListAsync());
        }

        // GET: DetailOrderModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetailOrders == null)
            {
                return NotFound();
            }

            var detailOrderModel = await _context.DetailOrders
                .Include(d => d.Order)
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.DetailId == id);
            if (detailOrderModel == null)
            {
                return NotFound();
            }

            return View(detailOrderModel);
        }

        // GET: DetailOrderModels/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: DetailOrderModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetailId,ProductId,OrderId,Money,Quantity")] DetailOrderModel detailOrderModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailOrderModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", detailOrderModel.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", detailOrderModel.ProductId);
            return View(detailOrderModel);
        }

        // GET: DetailOrderModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetailOrders == null)
            {
                return NotFound();
            }

            var detailOrderModel = await _context.DetailOrders.FindAsync(id);
            if (detailOrderModel == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", detailOrderModel.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", detailOrderModel.ProductId);
            return View(detailOrderModel);
        }

        // POST: DetailOrderModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DetailId,ProductId,OrderId,Money,Quantity")] DetailOrderModel detailOrderModel)
        {
            if (id != detailOrderModel.DetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailOrderModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailOrderModelExists(detailOrderModel.DetailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", detailOrderModel.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", detailOrderModel.ProductId);
            return View(detailOrderModel);
        }

        // GET: DetailOrderModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetailOrders == null)
            {
                return NotFound();
            }

            var detailOrderModel = await _context.DetailOrders
                .Include(d => d.Order)
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.DetailId == id);
            if (detailOrderModel == null)
            {
                return NotFound();
            }

            return View(detailOrderModel);
        }

        // POST: DetailOrderModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetailOrders == null)
            {
                return Problem("Entity set 'FashionContext.DetailOrders'  is null.");
            }
            var detailOrderModel = await _context.DetailOrders.FindAsync(id);
            if (detailOrderModel != null)
            {
                _context.DetailOrders.Remove(detailOrderModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetailOrderModelExists(int id)
        {
          return (_context.DetailOrders?.Any(e => e.DetailId == id)).GetValueOrDefault();
        }
    }
}
