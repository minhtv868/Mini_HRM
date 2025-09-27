using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class Media : BaseAuditableEntity
    {
        public int MediaId { get; set; }
        public byte MediaTypeId { get; set; }       // Image, Video, Audio, Doc...
        public short MediaGroupId { get; set; }     // Album, Category...
        public string MediaName { get; set; }
        public string MediaDesc { get; set; }
        public string EmbedCode { get; set; }
        public string OriginalFileName { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }
        public int FileSize { get; set; }
        public int? Width { get; set; }             // với ảnh/video
        public int? Height { get; set; }
        public int? Duration { get; set; }          // với audio/video
        public byte? Status { get; set; }
        public int? SiteId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
    }
}