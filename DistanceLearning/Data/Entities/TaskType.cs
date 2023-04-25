using System.ComponentModel.DataAnnotations;

namespace DistanceLearning.Data.Entities;

public class TaskType
{
    public int Id { get; set; }
    [Display(Name = "Название")]
    public string? Name { get; set; }

    public List<ExamTask> ExamTasks { get; set; } = new();
}
