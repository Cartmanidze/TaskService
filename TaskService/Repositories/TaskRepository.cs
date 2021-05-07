using GenericRepository.EFCore.Repositories;
using TaskService.Context;
using TaskService.Models;

namespace TaskService.Repositories
{
    public class TaskRepository : GenericRepository<TaskContext, TextTask>, ITaskRepository
    {

        public TaskRepository(TaskContext context) : base(context)
        {
        }
    }
}
