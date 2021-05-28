using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskService.Dto;
using TaskService.Models;
using TaskService.Repositories;

namespace TaskService.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;

        private readonly ITaskRepository _taskRepository;

        private readonly ITaskRunnerService _taskRunnerService;

        private readonly ILogger<TaskService> _logger;

        public TaskService(IMapper mapper, ITaskRepository taskRepository,  ILogger<TaskService> logger, ITaskRunnerService taskRunnerService)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
            _logger = logger;
            _taskRunnerService = taskRunnerService;
        }

        public async Task PutTaskAndStartAsync(TextTaskDto textTaskDto, CancellationToken token)
        {
            var textTask = _mapper.Map<TextTaskDto, TextTask>(textTaskDto);
            await _taskRepository.CreateAsync(textTask, token);
            await _taskRepository.SaveAsync(token);
#pragma warning disable 4014
            _taskRunnerService.RunTask(textTask, token);
#pragma warning restore 4014
            _logger.LogInformation($"Task with id = {textTask.Oid} running...");
        }

        public async Task<IEnumerable<TextTaskResultDto>> GetTaskResultsAsync(Guid id, CancellationToken token)
        {
            var textTasks = await _taskRepository.GetAsync(t => t.Oid == id, includeProperties: "TextTaskResults", token: token);
            var textTaskResults =
                _mapper.Map<IEnumerable<TextTaskResult>, IEnumerable<TextTaskResultDto>>(textTasks.SelectMany(t => t.TextTaskResults));
            return textTaskResults;
        }
    }
}

