using DistanceLearning.Data;
using DistanceLearning.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistanceLearning.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class GeneratedVariableController : Controller
    {
        private readonly AppDbContext _context;

        public GeneratedVariableController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GeneratedVariable genVariable)
        {
            if (ModelState.IsValid)
            {
                if (null == genVariable.Name || 0 == genVariable.UpperBound || 0 == genVariable.LowerBound)
                {
                    TempData["messageType"] = $"alert-danger";
                    TempData["message"] = $"Заполните поля";

                    return RedirectToAction("Edit", "ExamTasks", new { id = genVariable.TaskId });
                }
                _context.GeneratedVariables.Add(genVariable);
                await _context.SaveChangesAsync();

                TempData["messageType"] = $"alert-success";
                TempData["message"] = $"Запись сохранена";

                return RedirectToAction("Edit", "ExamTasks", new { id = genVariable.TaskId });
            }

            TempData["messageType"] = $"alert-danger";
            TempData["message"] = $"Ошибка";

            return RedirectToAction("Edit", "ExamTasks", new { id = genVariable.TaskId });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GeneratedVariable genVariable)
        {
            if (ModelState.IsValid)
            {
                if (null == genVariable.Name || 0 == genVariable.UpperBound || 0 == genVariable.LowerBound)
                {
                    TempData["messageType"] = $"alert-danger";
                    TempData["message"] = $"Заполните поля";

                    return RedirectToAction("Edit", "ExamTasks", new { id = genVariable.TaskId });
                }
                _context.GeneratedVariables.Update(genVariable);
                await _context.SaveChangesAsync();

                TempData["messageType"] = $"alert-success";
                TempData["message"] = $"Запись сохранена";

                return RedirectToAction("Edit", "ExamTasks", new { id = genVariable.TaskId });
            }

            TempData["messageType"] = $"alert-danger";
            TempData["message"] = $"Ошибка";

            return RedirectToAction("Edit", "ExamTasks", new { id = genVariable.TaskId });
        }

        [HttpPost]
        public IActionResult Delete(int deletedId)
        {
            var deleted = _context.GeneratedVariables.FirstOrDefault(c => c.Id == deletedId);
            if (deleted != null)
            {
                _context.GeneratedVariables.Remove(deleted);
                _context.SaveChanges();

                TempData["messageType"] = $"alert-success";
                TempData["message"] = $"Запись была удалена";
            }

            return RedirectToAction("Edit", "ExamTasks", new { id = deleted.TaskId });
        }
    }
}
