//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using MediatR;
//using System.ComponentModel;
//using System.Linq.Dynamic.Core;
//using Web.Application.DTOs.MediatR;
//using Web.Application.Extensions;
//using Web.Application.Features.Finance.Leagues.DTOs;
//using Web.Application.Interfaces;
//using Web.Application.Interfaces.Repositories.Finances;
//using Web.Domain.Entities.Finance;
//using Web.Shared;

//namespace Web.Application.Features.Finance.Leagues.Queries
//{
//    public record LeagueGetPageQuery : BaseGetPageQuery, IRequest<PaginatedResult<LeagueGetPageDto>>
//    {
//        [DisplayName("Site")]
//        public short? SiteId { get; set; }
//        [DisplayName("Hình thức gửi")]
//        public byte SendMethodId { get; set; }


//    }
//    internal class LeagueGetPageQueryHandler : IRequestHandler<LeagueGetPageQuery, PaginatedResult<LeagueGetPageDto>>
//    {
//        private readonly IFinanceUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        private readonly ISender _sender;
//        private readonly IAuditableService _auditableService;
//        public LeagueGetPageQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender, IAuditableService auditableService)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//            _sender = sender;
//            _auditableService = auditableService;
//        }

//        public async Task<PaginatedResult<LeagueGetPageDto>> Handle(LeagueGetPageQuery queryInput, CancellationToken cancellationToken)
//        {
//            var query = _unitOfWork.Repository<League>().Entities;
//            // var listSendMethod = _unitOfWork.Repository<SendMethod>().Entities;
//            if (queryInput.SiteId > 0)
//            {
//                query = query.Where(x => x.SiteId == queryInput.SiteId);
//            }
//            //if (queryInput.SendMethodId > 0)
//            //{
//            //    query = query.Where(x => x.SendMethodId == queryInput.SendMethodId);
//            //}
//            //if (!string.IsNullOrWhiteSpace(queryInput.Keywords))
//            //{
//            //    query = query.Where(x => x.MessageName.Contains(queryInput.Keywords) || x.SendFrom.Contains(queryInput.Keywords) || x.Title.Contains(queryInput.Keywords));
//            //}
//            var result = await query.OrderBy(x => x.CrDateTime).ProjectTo<LeagueGetPageDto>(_mapper.ConfigurationProvider).ToPaginatedListAsync(queryInput.Page, queryInput.PageSize, cancellationToken);
//            //if (result.Data != null && result.Data.Any())
//            //{
//            //    var listUsers = await _sender.Send(new UserGetAllQuery());
//            //    foreach (var item in result.Data)
//            //    {
//            //        //if (item.SendMethodId > 0)
//            //        //{
//            //        //    item.SendMethod = listSendMethod.FirstOrDefault(x => x.SendMethodId == item.SendMethodId).SendMethodName;
//            //        //}
//            //        if (item.CrUserId > 0)
//            //        {
//            //            var crUser = listUsers.Data.FirstOrDefault(x => x.Id == item.CrUserId);
//            //            item.CrUser = crUser != null ? crUser.UserName : "...";
//            //        }
//            //    }
//            //}
//            await _auditableService.UpdateAuditableInfoAsync(result.Data);
//            return result;
//        }
//    }
//}
