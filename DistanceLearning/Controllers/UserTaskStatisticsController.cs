using DistanceLearning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistanceLearning.Data;

namespace DistanceLearning.Controllers;

[Authorize(Roles = Data.Authorization.Constants.Roles.STUDENT)]
public class UserTaskStatisticsController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public UserTaskStatisticsController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var statisticList = new List<UserTaskStatisticsViewModel>();

        var user = _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult();

        var statics = _context.UserTaskStatistics.Include(e => e.ExamTask).Where(s => s.IdentityUser == user).OrderBy(s => s.CreateDate);

        var staticsGroup = statics
                .GroupBy(p => p.CreateDate)
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count(),
                    Data = g.Select(p => p)
                });

        foreach (var staticG in staticsGroup)
        {
            var statistic = new UserTaskStatisticsViewModel
            {
                Date = staticG.Name,
                Count = staticG.Count,
            };

            var rightAnswers = 0;
            var solutionChecking = 0;
            foreach (var staticData in staticG.Data)
            {
                if (staticData.Answer == staticData.RightAnswer)
                {
                    rightAnswers++;
                }
                if(1 == staticData.SolutionChecking)
                {
                    solutionChecking++;
                }
                statistic.UserTaskStatistic.Add(staticData);
            }
            statistic.CorrectlySolved = rightAnswers - solutionChecking;
            statistic.SolutionChecking = solutionChecking;

            statisticList.Add(statistic);
        }

        return View(statisticList);
    }

    public IActionResult Full()
    {
        var user = _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult();

        var statics = _context.UserTaskStatistics.Include(e => e.ExamTask).Where(s => s.IdentityUser == user).OrderBy(s => s.CreateDate);

        return View(statics);
    }
}
