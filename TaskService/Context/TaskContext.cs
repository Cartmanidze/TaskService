using Microsoft.EntityFrameworkCore;
using TaskService.Models;

namespace TaskService.Context
{
    public sealed class TaskContext : DbContext
    {
        public DbSet<TextTask> TextTasks { get; set; }

        public DbSet<TextTaskResult> TextTaskResults { get; set; }

        public TaskContext()
        {

        }

        public TaskContext(DbContextOptions<TaskContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
