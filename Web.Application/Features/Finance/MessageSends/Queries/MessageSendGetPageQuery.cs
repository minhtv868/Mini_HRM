using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Web.Application.DTOs.MediatR;
using Web.Application.Extensions;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.MessageSends.Queries
{
    public record MessageSendGetPageQuery : BaseGetPageQuery, IRequest<PaginatedResult<MessageSendGetPageDto>>
    {
        [DisplayName("Site")]
        public short? SiteId { get; set; }
        [DisplayName("Hình thức gửi")]
        public byte SendMethodId { get; set; }
        [DisplayName("Mẫu tin")]
        public byte MessageTemplateId { get; set; }
        [DisplayName("Trạng thái")]
        public byte SendStatusId { get; set; }
        [DisplayName("Từ ngày")]
        public string DateFrom { get; set; }
        [DisplayName("Đến ngày")]
        public string DateTo { get; set; }
        [DisplayName("Sắp xếp")]
        public string OrderBy { get; set; }
    }
    internal class MessageSendGetPageQueryHandler : IRequestHandler<MessageSendGetPageQuery, PaginatedResult<MessageSendGetPageDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public MessageSendGetPageQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<PaginatedResult<MessageSendGetPageDto>> Handle(MessageSendGetPageQuery queryInput, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<MessageSend>().Entities.AsNoTracking();
            if (queryInput.SiteId > 0)
            {
                query = query.Where(x => x.SiteId == queryInput.SiteId);
            }
            if (queryInput.SendMethodId > 0)
            {
                query = query.Where(x => x.SendMethodId == queryInput.SendMethodId);
            }
            if (queryInput.MessageTemplateId > 0)
            {
                query = query.Where(x => x.MessageTemplateId == queryInput.MessageTemplateId);
            }
            if (queryInput.SendStatusId > 0)
            {
                query = query.Where(x => x.SendStatusId == queryInput.SendStatusId);
            }
            //DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            //if (!string.IsNullOrEmpty(queryInput.DateFrom))
            //{
            //    dateFrom = queryInput.DateFrom.StrToDateTime().AddDays(-1);
            //}
            //if (!string.IsNullOrEmpty(queryInput.DateTo))
            //{
            //    dateTo = queryInput.DateTo.StrToDateTime().AddDays(-1);
            //}
            //if (dateFrom != DateTime.MinValue && dateTo != DateTime.MinValue)
            //{
            //    query = query.Where(x => x.CrDateTime >= dateFrom && x.CrDateTime <= dateTo);
            //}
            //else if (dateFrom != DateTime.MinValue)
            //{
            //    query = query.Where(x => x.CrDateTime >= dateFrom);
            //}
            //else if (dateTo != DateTime.MinValue)
            //{
            //    query = query.Where(x => x.CrDateTime <= dateTo);
            //}
            if (!string.IsNullOrEmpty(queryInput.OrderBy))
            {
                query = query.OrderBy(x => queryInput.OrderBy);
            }
            else
            {
                query = query.OrderByDescending(x => x.MessageSendId);
            }
            var result = await query
                .ProjectTo<MessageSendGetPageDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(queryInput.Page, queryInput.PageSize, cancellationToken);
            if (result.Data != null && result.Data.Any())
            {
                List<byte> sendMethodIds = result.Data.Select(x => x.SendMethodId).ToList();
                //  List<SendMethod> sendMethod = new List<SendMethod>();

                //if (sendMethodIds != null && sendMethodIds.Any())
                //{
                //    sendMethod = _unitOfWork.Repository<SendMethod>().Entities.Where(x => sendMethodIds.Contains(x.SendMethodId)).ToList();
                //}
                //foreach (var item in result.Data)
                //{
                //    if (item.SendMethodId > 0)
                //    {
                //        item.SendMethodName = sendMethod.FirstOrDefault(x => x.SendMethodId == item.SendMethodId).SendMethodName;
                //    }
                //    if (item.SendStatusId > 0)
                //    {
                //        item.SendStatusName = ((SendStatusEnum)item.SendStatusId).GetDisplayName();
                //    }
                //}

            }
            return result;
        }
    }
}
