using AutoMapper;
using Web.Application.Common.Mappings;
using Web.Application.DTOs.MediatR;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.BongDa24hCrawls;
using Web.Domain.Entities.Crawls;
using Web.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Web.Application.Features.BongDa24hCrawls.TemporaryDatas.Commands
{
    public record TemporaryDataCreateCommand : BaseCreateCommand, IRequest<Result<int>>, IMapFrom<TemporaryData>
    {
        public int Id { get; set; }
        public string DataSouceName { get; set; }
        public string DataId { get; set; }
        public string DataAction { get; set; }
        public string DataJson { get; set; }
        public long? Hash { get; set; }
        public string BatchCode { get; set; }
    }
    internal class TemporaryDataCreateCommandHandler : IRequestHandler<TemporaryDataCreateCommand, Result<int>>
    {
        private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public TemporaryDataCreateCommandHandler(IBongDa24HCrawlUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(TemporaryDataCreateCommand command, CancellationToken cancellationToken)
        {
            var isExists = await _unitOfWork.Repository<TemporaryData>().Entities.AnyAsync(x => x.Hash == command.Hash);

            if (isExists)
            {
                return await Result<int>.FailureAsync("Đã tồn tại.");
            }

            var entity = _mapper.Map<TemporaryData>(command);
            entity.CrDateTime = DateTime.Now;
            await _unitOfWork.Repository<TemporaryData>().AddAsync(entity);
            await _unitOfWork.Save(cancellationToken);
            return await Result<int>.SuccessAsync(entity.Id, "Thành công.");
        }
    }
}
