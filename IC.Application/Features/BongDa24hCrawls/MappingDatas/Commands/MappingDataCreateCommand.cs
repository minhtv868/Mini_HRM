using AutoMapper;
using IC.Application.Common.Mappings;
using IC.Application.DTOs.MediatR;
using IC.Application.Interfaces;
using IC.Application.Interfaces.Repositories.BongDa24hCrawls;
using IC.Domain.Entities.BongDa24hCrawls;
using IC.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IC.Application.Features.BongDa24hCrawls.MappingDatas.Commands
{
    public record MappingDataCreateCommand : BaseCreateCommand, IRequest<Result<int>>, IMapFrom<MappingData>
    {
        public int Id { get; set; }
        public string DataSouceName { get; set; }
        public int DataId { get; set; }
        public string DataKey { get; set; }
        public DateTime? CrDateTime { get; set; }
    }
    internal class MappingDataCreateCommandHandler : IRequestHandler<MappingDataCreateCommand, Result<int>>
    {
        private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public MappingDataCreateCommandHandler(IBongDa24HCrawlUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(MappingDataCreateCommand command, CancellationToken cancellationToken)
        {
            var isExists = await _unitOfWork.Repository<MappingData>().Entities.AnyAsync(x => x.DataSouceName == command.DataSouceName && x.DataId == command.DataId && x.DataKey == command.DataKey);

            if (isExists)
            {
                return await Result<int>.FailureAsync("Đã tồn tại.");
            }

            var entity = _mapper.Map<MappingData>(command);
            entity.CrDateTime = DateTime.Now;
            await _unitOfWork.Repository<MappingData>().AddAsync(entity);
            await _unitOfWork.Save(cancellationToken);
            return await Result<int>.SuccessAsync(entity.Id, "Thành công.");
        }
    }
}
