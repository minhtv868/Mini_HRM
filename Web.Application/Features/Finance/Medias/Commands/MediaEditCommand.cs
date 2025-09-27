using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Features.Finance.Medias.DTOs;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Medias.Commands
{
    public class MediaEditCommand : IRequest<Result<int>>, IMapFrom<Media>, IMapFrom<MediaGetByIdDto>
    {
        public int MediaId { get; set; }
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
    }
    internal class MediaEditCommandHandler : IRequestHandler<MediaEditCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public MediaEditCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService,
            ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(MediaEditCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Media>().Entities.AsNoTracking().FirstOrDefault(x => x.MediaId == command.MediaId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("Media không tồn tại");
            }
            if (command.FilePath != entity.FilePath)
            {
                var existing = await _unitOfWork.Repository<Media>().Entities
             .Where(x => x.SiteId == command.SiteId)
             .AsNoTracking()
             .ToListAsync();
                var existing2 = existing.FirstOrDefault(x => string.Equals(x.FilePath, command.FilePath, StringComparison.Ordinal));
                if (existing2 != null)
                {
                    return await Result<int>.FailureAsync("Media này đã tồn tại. Vui lòng chọn tên khác.");
                }
            }
            entity = _mapper.Map<Media>(command);
            entity.UpdDateTime = DateTime.Now;
            entity.UpdUserId = _currentUserService.UserId;
            await _unitOfWork.Repository<Media>().UpdateFieldsAsync(entity,
                x => x.FilePath,
                x => x.MediaTypeId,
                x => x.MediaGroupId,
                x => x.MediaName,
                x => x.MediaDesc,
                x => x.EmbedCode,
                x => x.UpdUserId,
                x => x.UpdDateTime
              );
            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync("Cập nhật dữ liệu thành công.");
            }
            return await Result<int>.FailureAsync("Cập nhật dữ liệu không thành công.");
        }
    }
}
