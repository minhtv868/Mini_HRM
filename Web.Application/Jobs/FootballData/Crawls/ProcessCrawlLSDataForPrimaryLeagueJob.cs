
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Web.Application.Features.Finance.UrlCrawls.Queries;
using Web.Application.Helpers;
using Web.Application.Jobs.Helper;

namespace Web.Application.Jobs.FootballData.Crawls
{
    [DisplayName("[Job] Crawl dữ liệu giải đấu chính")]
    public record ProcessCrawlPrimaryLeagueJob() : IRequest
    {
    }

    internal class ProcessCrawlPrimaryLeagueJobHandler : IRequestHandler<ProcessCrawlPrimaryLeagueJob>
    {
        private readonly ILogger<ProcessCrawlPrimaryLeagueJobHandler> _logger;
        private readonly IMediator _mediator;

        public ProcessCrawlPrimaryLeagueJobHandler(ILogger<ProcessCrawlPrimaryLeagueJobHandler> logger,
          IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Handle(ProcessCrawlPrimaryLeagueJob command, CancellationToken cancellationToken)
        {
            if (JobRunningHelper.ProcessCrawlPrimaryLeagueJob)
            {
                return;
            }

            JobRunningHelper.ProcessCrawlPrimaryLeagueJob = true;
            try
            {
                await CrawlPrimaryLeague(cancellationToken);
            }
            finally
            {
                JobRunningHelper.ProcessCrawlPrimaryLeagueJob = false;
            }
        }

        private async Task CrawlPrimaryLeague(CancellationToken cancellationToken)
        {
            try
            {
                var listUrlCrawls = await _mediator.Send(new UrlCrawlGetAllQuery() { DataType = 1 }, cancellationToken);

                if (listUrlCrawls != null && listUrlCrawls.Any())
                {
                    var tasks = new List<Task>();
                    var semaphore = new SemaphoreSlim(20); // Tối đa 20 task song song

                    foreach (var item in listUrlCrawls)
                    {
                        await semaphore.WaitAsync(cancellationToken);

                        tasks.Add(Task.Run(async () =>
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(item.Url))
                                {
                                    _logger.LogInformation($"Crawling URL: {item.Url}");

                                    string data = await HtmlCrawer.CrawAsync(item.Url);

                                    if (!string.IsNullOrEmpty(data))
                                    {
                                        await ProcessMatchData(data, cancellationToken);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Error crawling URL: {item.Url}");
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        }, cancellationToken));
                    }

                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CrawlPrimaryLeague");
            }
        }

        private async Task ProcessMatchData(string jsonData, CancellationToken cancellationToken)
        {
            try
            {
                var matchCommands = FootballDataHelper.ParseLivescoreData(jsonData);

                _logger.LogInformation($"Parsed {matchCommands.Count} matches from data");

                //var tasks = matchCommands.Select(async command =>
                //{
                //    try
                //    {
                //        var result = await _mediator.Send(command, cancellationToken);
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError(ex, $"Error processing match");
                //    }
                //});

                //await Task.WhenAll(tasks);
                foreach (var command in matchCommands)
                {
                    try
                    {
                        var result = await _mediator.Send(command, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error processing match");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing match data");
            }
        }
    }
}
