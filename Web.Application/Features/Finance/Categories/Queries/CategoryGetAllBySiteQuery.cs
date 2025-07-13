using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Categories.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Categories.Queries
{
    public record CategoryGetAllBySiteQuery : IRequest<List<CategoryGetAllBySiteDto>>
    {
        public int? SiteId { get; set; }
        public byte CategoryLevel { get; set; }
        public string KeyWords { get; set; }
    }

    internal class CategoryGetAllBySiteQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CategoryGetAllBySiteQuery, List<CategoryGetAllBySiteDto>>
    {
        public async Task<List<CategoryGetAllBySiteDto>> Handle(CategoryGetAllBySiteQuery queryInput, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Repository<Category>().Entities.AsNoTracking()
            .Where(x => x.SiteId == queryInput.SiteId);
            if (queryInput.CategoryLevel > 0)
            {
                query = query.Where(x => x.CategoryLevel == queryInput.CategoryLevel);
            }

            if (!string.IsNullOrEmpty(queryInput.KeyWords))
            {
                query = query.Where(x => x.CategoryName.Contains(queryInput.KeyWords) || x.CategoryDesc.Contains(queryInput.KeyWords));
            }

            return await query
                .ProjectTo<CategoryGetAllBySiteDto>(mapper.ConfigurationProvider)
                .OrderBy(x => x.TreeOrder)
                .ToListAsync(cancellationToken);
        }
    }
}
