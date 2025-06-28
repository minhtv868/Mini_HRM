using AutoMapper;
using AutoMapper.QueryableExtensions;
using Web.Application.Interfaces.Repositories;
using Web.Domain.Entities.Identity;
using Web.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Web.Application.Features.IdentityFeatures.SysFunctions.Queries
{
	public record SysFunctionGetAllQuery : IRequest<Result<List<SysFunctionGetAllDto>>>
	{
	}

	internal class SysFunctionGetAllQueryHandler : IRequestHandler<SysFunctionGetAllQuery, Result<List<SysFunctionGetAllDto>>>
	{
		private readonly IIdentityUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public SysFunctionGetAllQueryHandler(IIdentityUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<List<SysFunctionGetAllDto>>> Handle(SysFunctionGetAllQuery query, CancellationToken cancellationToken)
		{
			var allSysFunctionsList = await _unitOfWork.Repository<SysFunction>().Entities
				   .OrderBy(x => x.TreeOrder)
				   .ProjectTo<SysFunctionGetAllDto>(_mapper.ConfigurationProvider)
				   .ToListAsync(cancellationToken);

			return await Result<List<SysFunctionGetAllDto>>.SuccessAsync(allSysFunctionsList);
		}
	}
}
