using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Categories.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Categories.Queries
{
    public class CategoryGetByParentIdQuery : IRequest<List<CategoryGetByParentIdDto>>
    {
        public int ParentCategoryId { get; set; }
        public int SiteId { get; set; }
    }
    internal class CategoryGetByParentIdQueryHandler : IRequestHandler<CategoryGetByParentIdQuery, List<CategoryGetByParentIdDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public CategoryGetByParentIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;

        }
        public async Task<List<CategoryGetByParentIdDto>> Handle(CategoryGetByParentIdQuery queryInput, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Category>().Entities;
            if (queryInput.ParentCategoryId >= 0)
            {
                query = query.Where(x => x.ParentCategoryId == queryInput.ParentCategoryId);
            }
            if (queryInput.SiteId >= 0)
            {
                query = query.Where(x => x.SiteId == queryInput.SiteId);
            }
            var result = await query.OrderBy(x => x.TreeOrder).ProjectTo<CategoryGetByParentIdDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    item.Id = item.CategoryId;
                }
            }
            return result;
        }
    }
}
