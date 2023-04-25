using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DistanceLearning.Data;
using DistanceLearning.Data.Entities;

namespace DistanceLearning.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class TaskTypesController : Controller
    {
        private readonly AppDbContext _context;

        public TaskTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/TaskTypes
        public async Task<IActionResult> Index()
        {
              return _context.TaskTypes != null ? 
                          View(await _context.TaskTypes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.TaskTypes'  is null.");
        }

        // GET: Admin/TaskTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TaskTypes == null)
            {
                return NotFound();
            }

            var taskType = await _context.TaskTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskType == null)
            {
                return NotFound();
            }

            return View(taskType);
        }

        // GET: Admin/TaskTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TaskTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TaskType taskType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskType);
        }

        // GET: Admin/TaskTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TaskTypes == null)
            {
                return NotFound();
            }

            var taskType = await _context.TaskTypes.FindAsync(id);
            if (taskType == null)
            {
                return NotFound();
            }
            return View(taskType);
        }

        // POST: Admin/TaskTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TaskType taskType)
        {
            if (id != taskType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskTypeExists(taskType.Id))
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
            return View(taskType);
        }

        // GET: Admin/TaskTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TaskTypes == null)
            {
                return NotFound();
            }

            var taskType = await _context.TaskTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskType == null)
            {
                return NotFound();
            }

            return View(taskType);
        }

        // POST: Admin/TaskTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TaskTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TaskTypes'  is null.");
            }
            var taskType = await _context.TaskTypes.FindAsync(id);
            if (taskType != null)
            {
                _context.TaskTypes.Remove(taskType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskTypeExists(int id)
        {
          return (_context.TaskTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
