//using AutoMapper;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Web.Application.Features.Finance.InterestRates.DTOs;
//using Web.Application.Interfaces.Repositories.Finances;
//using Web.Domain.Entities.Finance;

//namespace Web.Application.Features.Finance.InterestRates.Queries
//{
//    public class InterestRateGetAllBySiteQuery : IRequest<List<InterestRateGetAllBySiteDto>>
//    {

//    }
//    internal class InterestRateGetAllBySiteQueryHandler : IRequestHandler<InterestRateGetAllBySiteQuery, List<InterestRateGetAllBySiteDto>>
//    {
//        private readonly IFinanceUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        private readonly ISender _sender;
//        public InterestRateGetAllBySiteQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//            _sender = sender;
//        }
//        public async Task<List<BankRateGetAllBySiteDto>> Handle(InterestRateGetAllBySiteQuery request, CancellationToken cancellationToken)
//        {
//            var banks = await _unitOfWork.Repository<Bank>()
//                .Entities
//                .AsNoTracking()
//                .Include(b => b.InterestRates)
//                .Where(b => b.IsActive)
//                .ToListAsync(cancellationToken);

//            var result = banks.Select(b => new InterestRateGetAllBySiteDto
//            {
//                BankName = b.BankName,
//                RatesByTerm = b.InterestRates
//                    .GroupBy(r => r.TermMonths)
//                    .Select(g => g.OrderByDescending(x => x.EffectiveDate).First())
//                    .ToDictionary(r => r.TermMonths, r => (decimal?)r.InterestRateValue)
//            }).ToList();

//            return result;
//        }

//    }
//}
