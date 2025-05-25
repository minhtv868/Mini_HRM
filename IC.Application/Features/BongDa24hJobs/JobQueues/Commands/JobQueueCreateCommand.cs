using AutoMapper;
using IC.Application.Common.Mappings;
using IC.Application.Interfaces.Repositories.BongDa24hJobs;
using IC.Domain.Entities.BongDa24hJobs;
using IC.Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IC.Application.Features.BongDa24hJobs.JobQueues.Commands
{
    public record JobQueueCreateCommand : IRequest<int>, IMapFrom<JobQueue>
    {
        public string DataSouceName { get; set; }
        public string DataId { get; set; }
        public string DataJson { get; set; }
        public string JobName { get; set; }
        public int? CrUserId { get; set; }
    }
    internal class JobQueueCreateCommandHandler : IRequestHandler<JobQueueCreateCommand, int>
    {
        private readonly IBongDa24hJobUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public JobQueueCreateCommandHandler(IBongDa24hJobUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> Handle(JobQueueCreateCommand command, CancellationToken cancellationToken)
        {
            var hashData = string.Join('|', command.DataSouceName, command.DataId, command.DataJson, command.JobName);
            var hashId = StringHelper.CreateId(hashData, true, System.Text.Encoding.UTF8);

            var isExists = await _unitOfWork.Repository<JobQueue>().Entities.AnyAsync(x => x.Hash == hashId);

            if (isExists)
            {
                return 0;
            }
            try
            {
                var entity = _mapper.Map<JobQueue>(command);
                entity.Hash = hashId;
                entity.CrDateTime = DateTime.Now;
                entity.IsPublicJob = true;

                await _unitOfWork.Repository<JobQueue>().AddAsync(entity);
                await _unitOfWork.Save(cancellationToken);

                return entity.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
