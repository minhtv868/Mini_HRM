using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Categories.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;


namespace Web.Application.Features.Finance.Categories.Queries
{
    public record CategoryGetByIdQuery : IRequest<Result<CategoryGetByIdDto>>
    {
        public int CategoryId { get; set; }
    }

    internal class CategoryGetByIdQueryHandler : IRequestHandler<CategoryGetByIdQuery, Result<CategoryGetByIdDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CategoryGetByIdDto>> Handle(CategoryGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository<Category>().Entities
                .AsNoTracking()
                .Where(x => x.CategoryId == queryInput.CategoryId)
                .ProjectTo<CategoryGetByIdDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                return await Result<CategoryGetByIdDto>.FailureAsync($"Id <b>{queryInput.CategoryId}</b> không tồn tại.");
            }
            return await Result<CategoryGetByIdDto>.SuccessAsync(result);
        }
    }
}
