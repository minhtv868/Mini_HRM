using AutoMapper;
using AutoMapper.QueryableExtensions;
using IC.Application.Features.BongDa24hJobs.JobQueues.DTOs;
using IC.Application.Interfaces.Repositories.BongDa24hJobs;
using IC.Domain.Entities.BongDa24hJobs;
using IC.Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IC.Application.Features.BongDa24hJobs.JobQueues.Queries
{
    public class JobQueueGetTopQuery  : IRequest<List<JobQueueDto>>
    {
        public int PageSize { get; set; } = 10;
		public bool IsPublicJob { get; set; } = true;
		public JobQueueGetTopQuery(int pageSize,bool isPublicJob = true)
        {
            PageSize = pageSize;
            IsPublicJob = isPublicJob;

		}   
    }
    internal class JobQueueGetTopQueryHandler : IRequestHandler<JobQueueGetTopQuery, List<JobQueueDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBongDa24hJobUnitOfWork _unitOfWork;
        public JobQueueGetTopQueryHandler(IMapper mapper, IBongDa24hJobUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<JobQueueDto>> Handle(JobQueueGetTopQuery request, CancellationToken cancellationToken)
        {
            //Lấy theo lô để xử lý
            //Tạo mã theo lô BatchCode
            var batchCode = StringHelper.GenerateUniqId();
            var sql = $"UPDATE JobQueues SET BatchCode=N'{batchCode}' WHERE Id IN (SELECT TOP({request.PageSize}) Id FROM JobQueues WHERE BatchCode IS NULL AND IsPublicJob = {(request.IsPublicJob ? 1 : 0)})";
            var rowCount = await _unitOfWork.Repository<JobQueue>().ExecNoneQuerySql(sql);

            if(rowCount > 0)
            {
                var result = await _unitOfWork.Repository<JobQueue>().Entities.AsNoTracking()
                   .Where(x => x.BatchCode == batchCode)
                   .ProjectTo<JobQueueDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

                return result;
            }

            return new List<JobQueueDto>();
        }
    }
}
