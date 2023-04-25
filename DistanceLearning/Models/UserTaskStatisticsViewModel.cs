using DistanceLearning.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace DistanceLearning.Models
{
    public class UserTaskStatisticsViewModel
    {
        [Display(Name = "Дата прохождения")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Display(Name = "Правильно решенные")]
        public int CorrectlySolved { get; set; }
        [Display(Name = "Использование решения")]
        public int SolutionChecking { get; set; }
        [Display(Name = "Количество")]
        public int Count { get; set; }

        public List<UserTaskStatistic> UserTaskStatistic { get; set; } = new();
    }
}
