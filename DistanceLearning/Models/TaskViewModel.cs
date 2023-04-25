using DistanceLearning.Data.Entities;

namespace DistanceLearning.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string? Text { get; set; }

        public Solution Solution { get; set; } = new Solution();
        public int SolutionChecking { get; set; }
        public string? RightAnswer { get; set; }
        public string? Answer { get; set; }

        public string? ImageName { get; set; }
        public string? ImagePath { get; set; }

        public int TaskTypeId { get; set; }

        public int Right { get; set; }

        public string? CardColor { get; set; }

    }
}
