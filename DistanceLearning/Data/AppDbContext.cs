using DistanceLearning.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DistanceLearning.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskType> TaskTypes => Set<TaskType>();
    public DbSet<ExamTask> ExamTasks => Set<ExamTask>();
    public DbSet<Solution> Solutions => Set<Solution>();
    public DbSet<GeneratedVariable> GeneratedVariables => Set<GeneratedVariable>();
    public DbSet<UserTaskStatistic> UserTaskStatistics => Set<UserTaskStatistic>();
}