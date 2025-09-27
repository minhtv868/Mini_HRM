using AutoMapper;
using MediatR;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Medias.Commands
{
    public class MediaCreateCommand : IRequest<Result<int>>, IMapFrom<Media>
    {
        [DisplayName("Loại")]
        public byte MediaTypeId { get; set; }       // Image, Video, Audio, Doc...
        [DisplayName("Nhóm")]
        public short MediaGroupId { get; set; }     // Album, Category...

        [DisplayName("Tên")]
        public string MediaName { get; set; }

        [DisplayName("Mô tả")]
        public string MediaDesc { get; set; }

        [DisplayName("Mã nhúng")]
        public string EmbedCode { get; set; }

        [DisplayName("Tên gốc")]
        public string OriginalFileName { get; set; }

        [DisplayName("Định dạng")]
        public string FileExtension { get; set; }
        [DisplayName("Đường dẫn")]
        public string FilePath { get; set; }
        [DisplayName("Kích thước")]
        public int FileSize { get; set; }
        [DisplayName("Rộng")]
        public int? Width { get; set; }

        [DisplayName("Dài")]// với ảnh/video
        public int? Height { get; set; }

        [DisplayName("Thời lượng")]
        public int? Duration { get; set; }          // với audio/video

        [DisplayName("Trạng thái")]
        public byte? Status { get; set; }

        [DisplayName("Site")]
        public int? SiteId { get; set; }

        [DisplayName("Thêm tiếp dữ liệu khác")]
        public bool AddMoreData { get; set; }
    }
    internal class MediaCreateCommandHandler : IRequestHandler<MediaCreateCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public MediaCreateCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(MediaCreateCommand command, CancellationToken cancellationToken)
        {
            var entityAny = _unitOfWork.Repository<Media>().Entities.FirstOrDefault(x => x.MediaName.Trim().ToLower().Equals(command.MediaName.Trim().ToLower()));
            if (entityAny != null)
            {
                return await Result<int>.FailureAsync($"Media đã tồn tại");
            }
            var entity = _mapper.Map<Media>(command);
            entity.CrUserId = _currentUserService.UserId;
            entity.CrDateTime = DateTime.Now;
            await _unitOfWork.Repository<Media>().AddAsync(entity);
            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync($"Thêm dữ liệu thành công");
            }
            return await Result<int>.FailureAsync($"Thêm dữ liệu không thành công");
        }
    }
}
