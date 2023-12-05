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
    public class AgesModelsController : Controller
    {
        private readonly FashionContext _context;

        public AgesModelsController(FashionContext context)
        {
            _context = context;
        }

        // GET: AgesModels
        public async Task<IActionResult> Index()
        {
              return _context.AgesModels != null ? 
                          View(await _context.AgesModels.ToListAsync()) :
                          Problem("Entity set 'FashionContext.AgesModels'  is null.");
        }

        // GET: AgesModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AgesModels == null)
            {
                return NotFound();
            }

            var agesModel = await _context.AgesModels
                .FirstOrDefaultAsync(m => m.agesid == id);
            if (agesModel == null)
            {
                return NotFound();
            }

            return View(agesModel);
        }

        // GET: AgesModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AgesModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("agesid,agenumber")] AgesModel agesModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agesModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(agesModel);
        }

        // GET: AgesModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AgesModels == null)
            {
                return NotFound();
            }

            var agesModel = await _context.AgesModels.FindAsync(id);
            if (agesModel == null)
            {
                return NotFound();
            }
            return View(agesModel);
        }

        // POST: AgesModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("agesid,agenumber")] AgesModel agesModel)
        {
            if (id != agesModel.agesid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agesModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgesModelExists(agesModel.agesid))
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
            return View(agesModel);
        }

        // GET: AgesModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AgesModels == null)
            {
                return NotFound();
            }

            var agesModel = await _context.AgesModels
                .FirstOrDefaultAsync(m => m.agesid == id);
            if (agesModel == null)
            {
                return NotFound();
            }

            return View(agesModel);
        }

        // POST: AgesModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AgesModels == null)
            {
                return Problem("Entity set 'FashionContext.AgesModels'  is null.");
            }
            var agesModel = await _context.AgesModels.FindAsync(id);
            if (agesModel != null)
            {
                _context.AgesModels.Remove(agesModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgesModelExists(int id)
        {
          return (_context.AgesModels?.Any(e => e.agesid == id)).GetValueOrDefault();
        }
    }
}
