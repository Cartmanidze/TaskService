using System;
using System.Collections.Generic;
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
        [Fact]
        public async Task Post_ReturnsBadRequest_IfStartTimeMoreOrEqualsThanEndTime()
        {
            var mockTaskService = new Mock<ITaskService>();
            var textTask = new TextTaskDto {StartTime = DateTime.UtcNow.AddHours(1), EndTime = DateTime.UtcNow};
            var cancellationToken = CancellationToken.None;
            var taskController = new TaskController(mockTaskService.Object);
            var result = await taskController.Post(textTask, cancellationToken);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_IfEndTimeLessOrEqualsThanNow()
        {
            var mockTaskService = new Mock<ITaskService>();
            var textTask = new TextTaskDto { StartTime = DateTime.UtcNow.AddHours(-2), EndTime = DateTime.UtcNow.AddHours(-1) };
            var cancellationToken = CancellationToken.None;
            var taskController = new TaskController(mockTaskService.Object);
            var result = await taskController.Post(textTask, cancellationToken);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsBadRequest_IfIdIsNotGuid()
        {
            var mockTaskService = new Mock<ITaskService>();
            var taskController = new TaskController(mockTaskService.Object);
            var result = await taskController.GetById("52", CancellationToken.None);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsContentResultsNotFound_IfResultsIsEmptyCollection()
        {
            var id = Guid.NewGuid();
            var mockTaskService = new Mock<ITaskService>();
            mockTaskService.Setup(t => t.GetTaskResultsAsync(id, CancellationToken.None))
                .ReturnsAsync(new List<TextTaskResultDto>());
            var taskController = new TaskController(mockTaskService.Object);
            var result = await taskController.GetById(id.ToString(), CancellationToken.None);
            Assert.IsType<ContentResult>(result);
        }
    }
}
