using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskService.Dto;

namespace TaskService.Services
{
    public interface ITaskService
    {
        Task PutTaskAndStartAsync(TextTaskDto textTaskDto, CancellationToken token);

        Task<IEnumerable<TextTaskResultDto>> GetTaskResultsAsync(Guid id, CancellationToken token);
    }
}
