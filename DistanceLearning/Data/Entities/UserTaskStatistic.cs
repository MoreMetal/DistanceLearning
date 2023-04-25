using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DistanceLearning.Data.Entities;

public class UserTaskStatistic
{
    public int Id { get; set; }
    [Display(Name = "Название")]
    public string? Name { get; set; }
    [Display(Name = "Текст")]
    public string? Text { get; set; }
    [Display(Name = "Правильный Ответ")]
    public string? RightAnswer { get; set; }
    [Display(Name = "Ответ")]
    public string? Answer { get; set; }
    [Display(Name = "Использование решения")]
    public int SolutionChecking { get; set; }
    [Display(Name = "Дата создания")]
    [DataType(DataType.Date)]
    public DateTime CreateDate { get; set; }

    [Display(Name = "Задание")]
    public int ExamTaskId { get; set; }
    [Display(Name = "Задание")]
    public ExamTask? ExamTask { get; set; }

    [Display(Name = "Пользователь")]
    public IdentityUser? IdentityUser { get; set; }
}
