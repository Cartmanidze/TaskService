using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskService.Controllers;
using TaskService.Dto;
using TaskService.Services;
using Xunit;

namespace TaskService.Tests.UnitTests
{
    public class TaskControllerTests
    {
        public async Task Post_ReturnsBadRequest_IfStartTimeMoreOrEqualsThanEndTime()
        {
            var mockTaskService = new Mock<ITaskService>();
            var textTask = new TextTaskDto {StartTime = DateTime.UtcNow.AddHours(1), EndTime = DateTime.UtcNow};
            var cancellationToken = CancellationToken.None;
            var taskController = new TaskController(mockTaskService.Object);
            var result = await taskController.Post(textTask, cancellationToken);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        public async Task Post_ReturnsBadRequest_IfEndTimeLessOrEqualsThanNow()
        {
            var mockTaskService = new Mock<ITaskService>();
            var textTask = new TextTaskDto { StartTime = DateTime.UtcNow.AddHours(-2), EndTime = DateTime.UtcNow.AddHours(-1) };
            var cancellationToken = CancellationToken.None;
            var taskController = new TaskController(mockTaskService.Object);
            var result = await taskController.Post(textTask, cancellationToken);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        public async Task GetById_ReturnsBadRequest_IfIdIsNotGuid()
        {
            var mockTaskService = new Mock<ITaskService>();
            var taskController = new TaskController(mockTaskService.Object);
            var result = await taskController.GetById("52", CancellationToken.None);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
