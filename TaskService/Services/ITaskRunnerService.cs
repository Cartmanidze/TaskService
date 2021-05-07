using System.Threading;
using System.Threading.Tasks;
using TaskService.Models;

namespace TaskService.Services
{
    public interface ITaskRunnerService
    {
        Task RunTask(TextTask textTask, CancellationToken token);
    }
}
