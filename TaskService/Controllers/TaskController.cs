using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TaskService.Dto;
using TaskService.Services;

namespace TaskService.Controllers
{
    [Authorize]
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
            if (textTaskDto.StartTime >= textTaskDto.EndTime) return BadRequest("Start time more than or equal end time");
            if (textTaskDto.EndTime <= DateTime.Now) return BadRequest("Now time more than or equal end time");
            await _taskService.PutTaskAndStartAsync(textTaskDto, token);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetById(string id, CancellationToken token)
        {
            if (!Guid.TryParse(id, out var val)) return BadRequest("Id is not GUID");
            var results = await _taskService.GetTaskResultsAsync(val, token);
            return results.Any() ? Ok(results) : Content("Результатов по заданному идентификатору не найдено");
        }
    }
}
