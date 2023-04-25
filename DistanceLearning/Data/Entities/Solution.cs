using System.ComponentModel.DataAnnotations;

namespace DistanceLearning.Data.Entities;

public class Solution
{
    public int Id { get; set; }
    [Display(Name = "Название")]
    public string? Text { get; set; }

    [Display(Name = "Названия изобр.")]
    public string? ImageName { get; set; }
    [Display(Name = "Путь изобр.")]
    public string? ImagePath { get; set; }

    [Display(Name = "Задание")]
    public int TaskId { get; set; }
    public ExamTask? Task { get; set; }
}
