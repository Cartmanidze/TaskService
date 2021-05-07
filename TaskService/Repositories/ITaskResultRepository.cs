using GenericRepository.EFCore.Repositories;
using TaskService.Models;

namespace TaskService.Repositories
{
    public interface ITaskResultRepository : IGenericRepository<TextTaskResult>
    {
    }
}
