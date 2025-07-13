using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Linq.Dynamic.Core;
using Web.Application.DTOs.MediatR;
using Web.Application.Extensions;
using Web.Application.Features.Finance.Articles.DTOs;
using Web.Application.Features.Finance.Categories.Queries;
using Web.Application.Features.IdentityFeatures.Users.Queries;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Articles.Queries
{
    public record ArticleGetPageQuery : BaseGetPageQuery, IRequest<PaginatedResult<ArticleGetPageDto>>
    {
        public int? CategoryId { get; set; }
        public int? SiteId { get; set; }
        public byte? DataTypeId { get; set; }
        public byte? ArticleTypeId { get; set; }
        public byte ReviewStatusId { get; set; }
    }
    internal class ArticleGetPageQueryHandler : IRequestHandler<ArticleGetPageQuery, PaginatedResult<ArticleGetPageDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        private readonly IAuditableService _auditableService;
        public ArticleGetPageQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender, IAuditableService auditableService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
            _auditableService = auditableService;
        }

        public async Task<PaginatedResult<ArticleGetPageDto>> Handle(ArticleGetPageQuery queryInput, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Article>().Entities;
            // var listSendMethod = _unitOfWork.Repository<SendMethod>().Entities;
            if (queryInput.SiteId > 0)
            {
                query = query.Where(x => x.SiteId == queryInput.SiteId);
            }
            //if (queryInput.SendMethodId > 0)
            //{
            //    query = query.Where(x => x.SendMethodId == queryInput.SendMethodId);
            //}
            //if (!string.IsNullOrWhiteSpace(queryInput.Keywords))
            //{
            //    query = query.Where(x => x.MessageName.Contains(queryInput.Keywords) || x.SendFrom.Contains(queryInput.Keywords) || x.Title.Contains(queryInput.Keywords));
            //}
            var result = await query.OrderBy(x => x.CrDateTime).ProjectTo<ArticleGetPageDto>(_mapper.ConfigurationProvider).ToPaginatedListAsync(queryInput.Page, queryInput.PageSize, cancellationToken);
            if (result.Data != null && result.Data.Any())
            {
                var listUsers = await _sender.Send(new UserGetAllQuery());
                var listCategories = await _sender.Send(new CategoryGetAllBySiteQuery() { SiteId = queryInput.SiteId });
                foreach (var item in result.Data)
                {
                    //if (item.SendMethodId > 0)
                    //{
                    //    item.SendMethod = listSendMethod.FirstOrDefault(x => x.SendMethodId == item.SendMethodId).SendMethodName;
                    //}
                    if (item.CrUserId > 0)
                    {
                        var crUser = listUsers.Data.FirstOrDefault(x => x.Id == item.CrUserId);
                        item.CrUser = crUser != null ? crUser.UserName : "...";
                    }
                    if (item.CategoryId > 0)
                    {
                        var category = listCategories.FirstOrDefault(x => x.Id == item.CategoryId);
                        item.CategoryName = category != null ? category.CategoryName : "...";
                    }

                }
            }
            //  await _auditableService.UpdateAuditableInfoAsync(result.Data);
            return result;
        }
    }
}
