using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.MessageTemplates.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.MessageTemplates.Queries
{
    public class MessageTemplateGetAllBySiteQuery : IRequest<List<MessageTemplateGetAllBySiteDto>>
    {
        public short? SiteId { get; set; }
    }
    internal class MessageTemplateGetAllBySiteQueryHandler : IRequestHandler<MessageTemplateGetAllBySiteQuery, List<MessageTemplateGetAllBySiteDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public MessageTemplateGetAllBySiteQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<MessageTemplateGetAllBySiteDto>> Handle(MessageTemplateGetAllBySiteQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<MessageTemplate>().Entities.Where(x => x.SiteId == request.SiteId);
            var result = await query
                 .ProjectTo<MessageTemplateGetAllBySiteDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}
