using System.ComponentModel.DataAnnotations;

namespace DistanceLearning.Data.Entities;

public class ExamTask
{
    public int Id { get; set; }
    [Display(Name = "Текст")]
    public string? Text { get; set; }

    public IList<GeneratedVariable> GeneratedVariables { get; set; } = new List<GeneratedVariable>();
    [Display(Name = "Формула")]
    public string? Formula { get; set; }
    [Display(Name = "Решение")]
    public Solution? Solution { get; set; }
    [Display(Name = "Названия изобр.")]
    public string? ImageName { get; set; }
    [Display(Name = "Путь изобр.")]
    public string? ImagePath { get; set; }
    [Display(Name = "Тип")]
    public int TaskTypeId { get; set; }
    [Display(Name = "Тип")]
    public TaskType? TaskType { get; set; }
    [Display(Name = "Статистика")]
    public List<UserTaskStatistic> UserTaskStatistics { get; set; } = new();
}
