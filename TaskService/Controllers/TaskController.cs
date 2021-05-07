using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskService.Dto;
using TaskService.Services;

namespace TaskService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {

        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(TextTaskDto textTaskDto, CancellationToken token)
        {
            await _taskService.PutTaskAndStartAsync(textTaskDto, token);
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<TextTaskResultDto>> GetById(string id, CancellationToken token)
        {
            if (!Guid.TryParse(id, out var val)) return null;
            var results = await _taskService.GetTaskResultsAsync(val, token);
            return results;
        }
    }
}
