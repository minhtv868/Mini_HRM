using AutoMapper;
using Azure.Core;
using Hangfire;
using Web.Application.Features.BongDa24hJobs.JobQueues.DTOs;
using Web.Application.Features.BongDa24hJobs.JobQueues.Queries;
using Web.Application.Interfaces.Repositories.BongDa24hJobs;
using Web.Domain.Entities.Jobs;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Web.Application.Jobs.ProcessCommons
{
	[DisplayName("[Job] Xử lý job chi tiết trong bảng JobQueues với trạng thái Public")]
	public record JobQueueProcessJob : IRequest
	{

	}
	internal class JobQueueProcessJobHandler : IRequestHandler<JobQueueProcessJob>
	{
		private readonly IBongDa24hJobUnitOfWork _uow;
		private readonly IMediator _mediator;
		private ILogger<JobQueueProcessJob> _logger;
		private readonly IBackgroundJobClient _backgroundJobClient;
		IConfiguration _configuration;
		IMapper _mapper;
		public JobQueueProcessJobHandler(IBongDa24hJobUnitOfWork uow,
			IMapper mapper, IMediator mediator, IBackgroundJobClient backgroundJobClient,
			IConfiguration configuration, ILogger<JobQueueProcessJob> logger)
		{
			_uow = uow;
			_mediator = mediator;
			_logger = logger;
			_backgroundJobClient = backgroundJobClient;
			_configuration = configuration;
			_mapper = mapper;
		}

		[JobDisplayName("[Job] Xử lý job chi tiết trong bảng JobQueues")]
		public async Task Handle(JobQueueProcessJob command, CancellationToken cancellationToken)
		{
			if (JobRunningHelper.JobQueueProcessJobRunning)
			{
				return;
			}

			JobRunningHelper.JobQueueProcessJobRunning = true;

			try
			{
				var jobQueues = await _mediator.Send(new JobQueueGetTopQuery(20));

				if (jobQueues != null && jobQueues.Any())
				{
					string queueConfigs = _configuration.GetSection("AppSettings").GetValue<string>("HangfireQueues");

					if (string.IsNullOrEmpty(queueConfigs))
					{
						queueConfigs = "default";
					}
					foreach (var jobQueue in jobQueues)
					{
						Type t = Type.GetType(jobQueue.JobName);

						//Dữ liệu chuyển đi sang bên JOB Item
						/*
						 VD:
						Cần lấy cầu thủ của trận đấu đá xong để chuyển trạng thái thành chờ crawl dữ liệu cầu thủ 

						DataSouceName: Matchs
						DataAction: Update
						DataId: 1
						
						ProcessGetPlayerRefreshDataByMatchJob

						Tìm đến bảng Matchs lấy dữ liệu theo MatchId = 1 rồi Thực hiện lấy danh sách cầu thủ

						 */
						var jobParam = _mapper.Map<JobQueueParamDto>(jobQueue);

						IRequest job = (IRequest)Activator.CreateInstance(t, jobParam);

						_backgroundJobClient.Enqueue<ISender>(queueConfigs, bridge => bridge.Send(job, cancellationToken));
					}

                    //Xóa queue
                    //var sql = $"UPDATE JobQueues SET IsPublicJob=0 WHERE BatchCode=N'{jobQueues[0].BatchCode}'";
                    var sql = $"DELETE JobQueues WHERE BatchCode=N'{jobQueues[0].BatchCode}'";
                    await _uow.Repository<JobQueue>().ExecNoneQuerySql(sql);
                }
            }
			catch (Exception ex)
			{
				_logger.LogError(ex.InnerException.Message);
			}

			JobRunningHelper.JobQueueProcessJobRunning = false;
		}
	}
} 