using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Categories.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Categories.Queries
{
    public record CategorySearchQuery : IRequest<List<CategorySearchDto>>
    {
        public string Keywords { get; set; }

        public int SiteId { get; set; }
    }

    internal class CategorySearchQueryHandler(IFinanceUnitOfWork unitOfWork) : IRequestHandler<CategorySearchQuery, List<CategorySearchDto>>
    {
        public async Task<List<CategorySearchDto>> Handle(CategorySearchQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Repository<Category>().Entities.AsNoTracking()
                .Where(x => x.SiteId == request.SiteId);


            if (!string.IsNullOrEmpty(request.Keywords))
            {
                query = query.Where(x => x.CategoryName.Contains(request.Keywords) || x.CategoryDesc.Contains(request.Keywords));
            }

            return await query
                .OrderBy(x => x.TreeOrder)
                .Select(x => new CategorySearchDto(x.CategoryId, x.CategoryName, x.CategoryLevel))
                .ToListAsync(cancellationToken);
        }
    }
}
