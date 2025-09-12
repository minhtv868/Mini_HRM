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
        public int? SiteId { get; init; }
        public byte CategoryLevel { get; init; }
        public string? KeyWords { get; init; }
    }

    internal class CategoryGetAllBySiteQueryHandler(
        IFinanceUnitOfWork unitOfWork,
        IMapper mapper
    ) : IRequestHandler<CategoryGetAllBySiteQuery, List<CategoryGetAllBySiteDto>>
    {
        public async Task<List<CategoryGetAllBySiteDto>> Handle(CategoryGetAllBySiteQuery queryInput, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Repository<Category>().Entities.AsNoTracking();

            if (queryInput.SiteId.HasValue)
                query = query.Where(x => x.SiteId == queryInput.SiteId);

            // lọc theo level
            if (queryInput.CategoryLevel > 0)
                query = query.Where(x => x.CategoryLevel == queryInput.CategoryLevel);

            // lọc theo từ khóa
            if (!string.IsNullOrWhiteSpace(queryInput.KeyWords))
            {
                string keyword = queryInput.KeyWords.Trim();
                query = query.Where(x =>
                    EF.Functions.Like(x.CategoryName, $"%{keyword}%") ||
                    EF.Functions.Like(x.CategoryDesc, $"%{keyword}%"));
            }

            return await query
                .OrderBy(x => x.TreeOrder)
                .ProjectTo<CategoryGetAllBySiteDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
