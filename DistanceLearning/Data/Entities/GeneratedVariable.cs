using System.ComponentModel.DataAnnotations;

namespace DistanceLearning.Data.Entities;

public class GeneratedVariable
{
    public int Id { get; set; }
    [Display(Name = "Название")]
    public string? Name { get; set; }
    [Display(Name = "Нижняя граница")]
    public float LowerBound { get; set; }
    [Display(Name = "Верхняя граница")]
    public float UpperBound { get; set; }

    [Display(Name = "Задание")]
    public int TaskId { get; set; }
    [Display(Name = "Задание")]
    public ExamTask? Task { get; set; }
}
