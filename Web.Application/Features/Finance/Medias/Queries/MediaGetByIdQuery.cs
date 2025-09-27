using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.Medias.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Medias.Queries
{
    public class MediaGetByIdQuery : IRequest<MediaGetByIdDto>
    {
        public int MediaId { get; set; }
    }
    internal class MediaGetByIdQueryHandler : IRequestHandler<MediaGetByIdQuery, MediaGetByIdDto>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public MediaGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<MediaGetByIdDto> Handle(MediaGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Media>().Entities.FirstOrDefault(x => x.MediaId == queryInput.MediaId);
            if (entity == null)
            {
                return new MediaGetByIdDto();
            }
            var dataGetByIdDto = _mapper.Map<MediaGetByIdDto>(entity);
            return dataGetByIdDto;
        }
    }
}
