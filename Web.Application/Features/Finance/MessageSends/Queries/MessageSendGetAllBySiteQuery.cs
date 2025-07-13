using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Web.Application.Features.Finance.MessageSends.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.MessageSends.Queries
{
    public class MessageSendGetAllBySiteQuery : IRequest<List<MessageSendGetAllBySiteDto>>
    {
        public short SiteId { get; set; }
        public byte SendMethodId { get; set; }
        public int PageSize { get; set; } = 50;
    }
    internal class MessageSendGetAllBySiteQueryHandler : IRequestHandler<MessageSendGetAllBySiteQuery, List<MessageSendGetAllBySiteDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public MessageSendGetAllBySiteQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<MessageSendGetAllBySiteDto>> Handle(MessageSendGetAllBySiteQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<MessageSend>().Entities.AsNoTracking().Where(x => x.SendStatusId == 1 && x.MessageTemplateId >= 130);
            if (request.SiteId > 0)
            {
                query = query.Where(x => x.SiteId == request.SiteId);
            }

            if (request.SendMethodId > 0)
            {
                query = query.Where(x => x.SendMethodId == request.SendMethodId);
            }
            var result = await query.OrderBy(x => x.MessageTemplateId)
                 .ProjectTo<MessageSendGetAllBySiteDto>(_mapper.ConfigurationProvider).Take(request.PageSize)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}
