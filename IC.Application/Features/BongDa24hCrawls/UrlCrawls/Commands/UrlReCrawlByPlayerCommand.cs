using AutoMapper;
using IC.Application.Common.Mappings;
using IC.Domain.Entities.BongDa24hCrawls;
using IC.Shared.Helpers;
using IC.Shared;
using MediatR;
using IC.Application.Interfaces.Repositories.BongDa24hCrawls;
using Microsoft.EntityFrameworkCore;
using IC.Domain.Enums.BongDa24hCrawls;
using Microsoft.IdentityModel.Tokens;

namespace IC.Application.Features.BongDa24hCrawls.UrlCrawls.Commands
{
    public record UrlReCrawlByPlayerCommand : IRequest<Result<int>>, IMapFrom<UrlCrawl>, IMapFrom<UrlCrawlInsertOrUpdateCommand>
    {
        public int PlayerId { get; set; }
    }

    internal class UrlReCrawlByPlayerCommandHandler : IRequestHandler<UrlReCrawlByPlayerCommand, Result<int>>
    {
        private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public UrlReCrawlByPlayerCommandHandler(IBongDa24HCrawlUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result<int>> Handle(UrlReCrawlByPlayerCommand command, CancellationToken cancellationToken)
        {

            var fslPlayer = await _unitOfWork.Repository<FSPlayerCrawl>().Entities.AsNoTracking().FirstOrDefaultAsync(x => x.PlayerId == command.PlayerId, cancellationToken);
            if (fslPlayer == null)
            {
                return await Result<int>.FailureAsync("Không có data");
            }
            var result = await _mediator.Send(new UrlCrawlInsertOrUpdateCommand()
            {
                Url = fslPlayer.UrlCrawl,
                UrlDesc = fslPlayer.PlayerName,
                UrlType = "DetailPage",
                UrlGroup = UrlCrawlUrlGroupEnum.FSPlayerCareerTranferInjury,
                UrlHash = 0,
                IsCrawled = false
            });
            if (result.Succeeded)
            {
                return await Result<int>.SuccessAsync("Thêm mới thành công.");
            }
            return await Result<int>.FailureAsync("Thêm mới thất bại");
        }
    }
}
