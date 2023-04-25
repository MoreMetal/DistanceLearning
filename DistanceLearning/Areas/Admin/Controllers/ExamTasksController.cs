using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DistanceLearning.Data;
using DistanceLearning.Data.Entities;

namespace DistanceLearning.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ExamTasksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ExamTasksController(AppDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Admin/ExamTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExamTasks.Include(e => e.TaskType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/ExamTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ExamTasks == null)
            {
                return NotFound();
            }

            var examTask = await _context.ExamTasks
                .Include(e => e.TaskType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examTask == null)
            {
                return NotFound();
            }

            return View(examTask);
        }

        // GET: Admin/ExamTasks/Create
        public IActionResult Create()
        {
            var examTask = new ExamTask();
            ViewData["TaskTypeId"] = new SelectList(_context.TaskTypes, "Id", "Name");
            return View(examTask);
        }

        // POST: Admin/ExamTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(ExamTask examTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examTask);
                await _context.SaveChangesAsync();

                TempData["messageType"] = $"alert-success";
                TempData["message"] = $"Запись сохранена";

                return RedirectToAction("Edit", new { id = examTask.Id });
            }

            TempData["messageType"] = $"alert-danger";
            TempData["message"] = $"Ошибка";

            ViewData["TaskTypeId"] = new SelectList(_context.TaskTypes, "Id", "Name", examTask.TaskTypeId);
            return View(examTask);
        }

        // GET: Admin/ExamTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExamTasks == null)
            {
                return NotFound();
            }

            var examTask = await _context.ExamTasks
                .Include(e => e.GeneratedVariables)
                .Include(e => e.Solution)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (examTask == null)
            {
                return NotFound();
            }

            var solution = new Solution();
            if (examTask.Solution == null)
            {
                examTask.Solution = solution;
            }

            ViewData["TaskTypeId"] = new SelectList(_context.TaskTypes, "Id", "Name", examTask.TaskTypeId);
            return View(examTask);
        }

        // POST: Admin/ExamTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ExamTask examTask)
        {
            if (id != examTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamTaskExists(examTask.Id))
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
            ViewData["TaskTypeId"] = new SelectList(_context.TaskTypes, "Id", "Name", examTask.TaskTypeId);
            return View(examTask);
        }

        // GET: Admin/ExamTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ExamTasks == null)
            {
                return NotFound();
            }

            var examTask = await _context.ExamTasks
                .Include(e => e.TaskType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examTask == null)
            {
                return NotFound();
            }

            return View(examTask);
        }

        // POST: Admin/ExamTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ExamTasks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ExamTasks'  is null.");
            }
            var examTask = await _context.ExamTasks.FindAsync(id);
            if (examTask != null)
            {
                _context.ExamTasks.Remove(examTask);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExamTaskExists(int id)
        {
          return (_context.ExamTasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> UploadImg(IFormFile uploadImg, int examTaskId)
        {
            var oldPath = new string("00");

            if (uploadImg == null || uploadImg.Length == 0)
            {

                TempData["messageType"] = $"alert-danger";
                TempData["message"] = $"Изображение не выбрано";

                return RedirectToAction("Index");
            }

            var webPath = Guid.NewGuid() + uploadImg.FileName;

            var path = Path.Combine(
                        _appEnvironment.WebRootPath, "file", $"{webPath}");

            try
            {
                var item = _context.ExamTasks.FirstOrDefault(c => c.Id == examTaskId);

                if (item == null)
                {
                    item = new ExamTask();
                }

                if (null != item?.ImagePath)
                    oldPath = Path.Combine(
                        _appEnvironment.WebRootPath, item.ImagePath);

                item.ImageName = uploadImg.FileName;
                item.ImagePath = Path.Combine("file", webPath);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await uploadImg.CopyToAsync(stream);
                }
                _context.SaveChanges();

                TempData["messageType"] = $"alert-success";
                TempData["message"] = $"Изображение добавлено";
            }
            catch (Exception ex)
            {
                TempData["messageType"] = $"alert-danger";
                TempData["message"] = $"Ошибка при записи {ex}";

                return RedirectToAction("Index");
            }

            FileInfo fileInf = new FileInfo(oldPath);
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }

            return RedirectToAction("Edit", new { id = examTaskId });
        }
    }
}
