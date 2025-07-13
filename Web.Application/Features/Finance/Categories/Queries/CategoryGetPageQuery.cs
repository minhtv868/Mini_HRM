using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using Web.Application.DTOs.MediatR;
using Web.Application.Extensions;
using Web.Application.Features.Finance.Categories.DTOs;
using Web.Application.Features.IdentityFeatures.Users.Queries;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Categories.Queries
{
    public record CategoryGetPageQuery : BaseGetPageQuery, IRequest<PaginatedResult<CategoryGetPageDto>>
    {
        [DisplayName("Site")]
        public short SiteId { get; set; }
        [DisplayName("Trạng thái")]
        public byte ReviewStatusId { get; set; }
        [DisplayName("Loại dữ liệu")]
        public short DataTypeId { get; set; } = 1;
        public int Delete { get; set; }
        [DisplayName("Cấp")]
        public byte? CategoryLevel { get; set; }
        public byte CategoryMaxLevel { get; set; }
    }
    internal class CategoryGetPageQueryHandler : IRequestHandler<CategoryGetPageQuery, PaginatedResult<CategoryGetPageDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public CategoryGetPageQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<PaginatedResult<CategoryGetPageDto>> Handle(CategoryGetPageQuery queryInput, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Category>().Entities;

            if (queryInput.SiteId > 0)
            {
                query = query.Where(x => x.SiteId == queryInput.SiteId);
            }
            queryInput.CategoryMaxLevel = query?.DefaultIfEmpty().Max(x => x.CategoryLevel) ?? 1;

            if (queryInput.CategoryLevel.HasValue)
            {
                query = query.Where(x => x.CategoryLevel == queryInput.CategoryLevel);
            }
            if (queryInput.ReviewStatusId > 0)
            {
                query = query.Where(x => x.ReviewStatusId == queryInput.ReviewStatusId);
            }
            if (queryInput.DataTypeId > 0)
            {
                query = query.Where(x => x.DataTypeId == queryInput.DataTypeId);
            }
            if (!string.IsNullOrEmpty(queryInput.Keywords))
            {
                query = query.Where(x => x.CategoryName.Contains(queryInput.Keywords) || x.CategoryDesc.Contains(queryInput.Keywords));
            }
            query = query.OrderBy(x => x.TreeOrder);

            var result = await query.ProjectTo<CategoryGetPageDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(queryInput.Page, queryInput.PageSize, cancellationToken);

            if (result.Data != null && result.Data.Any())
            {
                var userList = await _sender.Send(new UserGetAllQuery());
                // var dataTypeList = await _sender.Send(new DataTypeGetAllQuery());
                //    var reviewStatusList = await _sender.Send(new ReviewStatusGetAllQuery());
                foreach (var item in result.Data)
                {
                    if (item.CrUserId > 0)
                    {
                        var crUser = userList.Data.FirstOrDefault(x => x.Id == item.CrUserId);
                        item.CrUserName = crUser != null ? crUser.UserName : "...";
                    }
                    //if (item.DataTypeId > 0)
                    //{
                    //    var dataType = dataTypeList.Data.FirstOrDefault(x => x.DataTypeId == item.DataTypeId);
                    //    item.DataTypeName = dataType != null ? dataType.DataTypeDesc : "";
                    //}
                    //if (item.ReviewStatusId > 0)
                    //{
                    //    var reviewStatus = reviewStatusList.FirstOrDefault(x => x.ReviewStatusId == item.ReviewStatusId);
                    //    item.ReviewStatusName = reviewStatus != null ? reviewStatus.ReviewStatusDesc : "";
                    //}
                }
            }
            return result;
        }
    }
}
