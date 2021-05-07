using GenericRepository.EFCore.Repositories;
using TaskService.Context;
using TaskService.Models;

namespace TaskService.Repositories
{
    public class TaskResultRepository : GenericRepository<TaskContext, TextTaskResult>, ITaskResultRepository
    {
        public TaskResultRepository(TaskContext context) : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
