using DistanceLearning.Data;
using DistanceLearning.Data.Entities;
using DistanceLearning.Services;
using DistanceLearning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace DistanceLearning.Controllers;

[Authorize(Roles = Data.Authorization.Constants.Roles.STUDENT)]
public class StudentTasksController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public StudentTasksController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index() 
    {
        var types = _context.TaskTypes.ToList();
        return View(types); 
    }

    public IActionResult Option(int quantity, int taskTypeId)
    {
        var tasksViewModel = new List<TaskViewModel>();
        Random rnd1 = new Random();

        var taskContext = new List<ExamTask>();
        taskContext = _context.ExamTasks.Include(e => e.GeneratedVariables).Include(e => e.Solution).Where(s => s.TaskTypeId == taskTypeId).ToList();

        if(quantity > taskContext.Count())
        {
            var taskToAdd = new ExamTask();
            while (quantity > taskContext.Count())
            {
                taskToAdd = taskContext.OrderBy(x => rnd1.Next()).First();
                taskContext.Add(taskToAdd);
            }
            
        }
        else
        {
            taskContext = taskContext.OrderBy(x => rnd1.Next()).Take(quantity).ToList();
        }

        taskContext = taskContext.OrderBy(x => rnd1.Next()).ToList();

        foreach (var task in taskContext)
        {
            var generatedVariables = new Dictionary<string, double>();
            Random rnd = new Random();

            var taskViewModel = new TaskViewModel{ 
                Id = task.Id,
                ImageName = task.ImageName,
                ImagePath = task.ImagePath,
                Solution = task.Solution,
                TaskTypeId = task.TaskTypeId,
                SolutionChecking = 0
            };

            var formula = task.Formula;
            var taskText = task.Text;

            ///генерация новых значений
            foreach (var genVar in task.GeneratedVariables)
            {
                var randomElement = rnd.NextDouble() * (genVar.UpperBound - genVar.LowerBound) + genVar.LowerBound;

                generatedVariables.Add(genVar.Name, Math.Round(randomElement, 1));
            }
            ///запись новых зачений в строки
            ///
            if(null != formula)
            {
                foreach (var genVar in generatedVariables)
                {
                    var startIndex = formula.IndexOf(genVar.Key);
                    var count = genVar.Key.Length;

                    if (0 <= startIndex)
                    {
                        formula = formula.Remove(startIndex, count);
                        formula = formula.Insert(startIndex, genVar.Value.ToString());
                    }
                }
                var answer = Math.Round(CalculatingService.Result(formula), 3);
                taskViewModel.RightAnswer = answer.ToString();
            }

            if (null != taskText)
            {
                foreach (var genVar in generatedVariables)
                {
                    var startIndex = taskText.IndexOf(genVar.Key);
                    var count = genVar.Key.Length;

                    if (0 <= startIndex)
                    {
                        taskText = taskText.Remove(startIndex, count);
                        taskText = taskText.Insert(startIndex, genVar.Value.ToString());
                    }
                }
                taskViewModel.Text = taskText;
            }

            tasksViewModel.Add(taskViewModel);
        }

        return View(tasksViewModel);
    }

    public IActionResult TaskChecking(List<TaskViewModel> tasks)
    {
        var user = _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult();
        var index = 1;

        foreach (var taskItem in tasks)
        {
            var userTaskStatistic = new UserTaskStatistic();

            userTaskStatistic.Name = $"Задание {index++} №{taskItem.Id}";
            userTaskStatistic.Text = taskItem.Text;
            userTaskStatistic.RightAnswer = taskItem.RightAnswer;
            userTaskStatistic.Answer = taskItem.Answer;
            userTaskStatistic.SolutionChecking = taskItem.SolutionChecking;
            userTaskStatistic.ExamTaskId = taskItem.Id;
            userTaskStatistic.CreateDate = DateTime.Now.Date;
            userTaskStatistic.IdentityUser = user;

            if(1 == taskItem.SolutionChecking)
            {
                taskItem.CardColor = "bg-warning";
            }
            else if (taskItem.Answer == taskItem.RightAnswer)
            {
                taskItem.Right = 1;
            }
            else {
                taskItem.CardColor = "bg-danger";
            }

            _context.UserTaskStatistics.Add(userTaskStatistic);
        }
        _context.SaveChanges();

        return View(tasks);
    }

    public List<string> VariableRecognition(string formula)
    {
        ///в конце нужна точка
        char[] stopElements = new char[] { '+', '-', '/', '*', ' ', '(', ')', '.', ',', '!', '?' };
        var valueList = new List<string>();

        for (int i = 0; i < formula.Length; i++)
        {
            if ('$' == formula[i])
            {
                var value = "";
                //считывает элемента
                while (!stopElements.Contains(formula[i]))
                {
                    value = value + formula[i];
                    i++;
                }

                valueList.Add(value);
            }
        }

        return valueList;
    }

    public string RemovingExtraSpaces(string str)
    {
        var sb = new StringBuilder(str.Length);
        int ind = 0;
        while (ind < str.Length)
        {
            while (ind < str.Length && str[ind] != ' ') sb.Append(str[ind++]);
            if (ind < str.Length && str[ind] == ' ')
                sb.Append(' ');
            while (ind < str.Length && str[ind] == ' ') ind++;
        }

        return sb.ToString();
    }
}
