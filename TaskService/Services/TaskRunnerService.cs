using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GrpcFind;
using GrpcText;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskService.Models;
using TaskService.Repositories;

namespace TaskService.Services
{
    public class TaskRunnerService : ITaskRunnerService
    {

        private readonly Find.FindClient _findClient;

        private readonly Text.TextClient _textClient;

        private readonly ILogger<TaskRunnerService> _logger;

        public IServiceProvider Services { get; }

        public TaskRunnerService(IServiceProvider services, Find.FindClient findClient, Text.TextClient textClient, ILogger<TaskRunnerService> logger)
        {
            Services = services;
            _findClient = findClient;
            _textClient = textClient;
            _logger = logger;
        }

        public async Task RunTask(TextTask textTask, CancellationToken token)
        {
            await Task.Run(async () => await RunTaskInner(textTask, token), token);
            _logger.LogInformation($"Task with id = {textTask.Oid} complete");
        }

        private async Task RunTaskInner(TextTask textTask, CancellationToken token)
        {
            var processedTextsIds = new List<string>();
            using var scope = Services.CreateScope();
            using var taskResultRepository = scope.ServiceProvider.GetService<ITaskResultRepository>() ?? throw new Exception("Cannot get task result repository");
            while (DateTime.Now < textTask.EndTime && !token.IsCancellationRequested)
            {
                if (DateTime.Now < textTask.StartTime) continue;
                try
                {
                    var responseTextAll = await _textClient.GetTextsAllAsync(new TextAllRequest());
                    var notProcessedTexts = responseTextAll.Items.Where(t => !processedTextsIds.Contains(t.Id));
                    foreach (var text in notProcessedTexts)
                    {
                        var responseFindWords = await _findClient.FindWordsAsync(new FindRequest
                            {TextId = text.Id, SearchWords = {textTask.SearchWords.Split(';')}});
                        var foundedWords = responseFindWords.FoundWords;
                        var textTaskResult = new TextTaskResult
                        {
                            FoundedWords = string.Join(";", foundedWords),
                            TextId = Guid.Parse(text.Id),
                            TextTaskOid = textTask.Oid,
                        };
                        await taskResultRepository.CreateAsync(textTaskResult, token);
                        await taskResultRepository.SaveAsync(token);
                        processedTextsIds.Add(text.Id);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
                finally
                {
                    await Task.Delay(textTask.Duration, token);
                }
            }
        }
    }
}
