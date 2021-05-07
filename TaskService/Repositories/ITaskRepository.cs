using System.Threading;
using System.Threading.Tasks;
using GenericRepository.EFCore.Repositories;
using TaskService.Models;

namespace TaskService.Repositories
{
    public interface ITaskRepository : IGenericRepository<TextTask>
    {
    }
}
