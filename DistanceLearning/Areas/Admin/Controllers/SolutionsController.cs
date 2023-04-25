using DistanceLearning.Data;
using DistanceLearning.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistanceLearning.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SolutionsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public SolutionsController(AppDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Solution item)
        {
            if (ModelState.IsValid)
            {
                if (null == item.Text)
                {
                    TempData["messageType"] = $"alert-danger";
                    TempData["message"] = $"Заполните поля";

                    return RedirectToAction("Edit", "ExamTasks", new { id = item.TaskId });
                }
                _context.Solutions.Update(item);
                await _context.SaveChangesAsync();

                TempData["messageType"] = $"alert-success";
                TempData["message"] = $"Запись сохранена";

                return RedirectToAction("Edit", "ExamTasks", new { id = item.TaskId });
            }

            TempData["messageType"] = $"alert-danger";
            TempData["message"] = $"Ошибка";

            return RedirectToAction("Edit", "ExamTasks", new { id = item.TaskId });
        }

        public async Task<IActionResult> UploadImg(IFormFile uploadImg, int solutionId, int examTaskId)
        {
            var oldPath = new string("00");
            var item = _context.Solutions.FirstOrDefault(c => c.Id == solutionId);

            if (uploadImg == null || uploadImg.Length == 0)
            {

                TempData["messageType"] = $"alert-danger";
                TempData["message"] = $"Изображение не выбрано";

                return RedirectToAction("Edit", "ExamTasks", new { id = examTaskId });
            }
            if (item == null)
            {

                TempData["messageType"] = $"alert-danger";
                TempData["message"] = $"Сохраните запись";

                return RedirectToAction("Edit", "ExamTasks", new { id = examTaskId });
            }

            var webPath = Guid.NewGuid() + uploadImg.FileName;

            var path = Path.Combine(
                        _appEnvironment.WebRootPath, "file", $"{webPath}");

            try
            {
                if (null != item?.ImagePath)
                    oldPath = Path.Combine(
                        _appEnvironment.WebRootPath, item.ImagePath);

                item.ImageName = uploadImg.FileName;
                item.ImagePath = Path.Combine("file", webPath);
                item.TaskId = examTaskId;

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

                return RedirectToAction("Edit", "ExamTasks", new { id = examTaskId });
            }

            FileInfo fileInf = new FileInfo(oldPath);
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }

            return RedirectToAction("Edit", "ExamTasks", new { id = examTaskId });
        }
    }
}
