using IC.Domain.Common;

namespace IC.Domain.Entities.Finance
{
    public class Article : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }            // Tiêu đề chính (hiển thị H1)
        public string Slug { get; set; }             // Đường dẫn URL (VD: "cach-tiet-kiem-cho-genz")
        public string Summary { get; set; }          // Mô tả ngắn (thường là đoạn đầu nội dung)
        public string Content { get; set; }
        public string MetaTitle { get; set; }        // <title>
        public string MetaDescription { get; set; }  // <meta name="description">
        public string ImageUrl { get; set; }         // og:image, ảnh đại diện
        public DateTime? PublishTime { get; set; }   // Ngày xuất bản
        public int ViewCount { get; set; } = 0;
        public int CommentCount { get; set; }
        public int? CategoryId { get; set; }
        public int? SiteId { get; set; }
        public byte? DataTypeId { get; set; }
        public byte? ArticleTypeId { get; set; }
        public byte ReviewStatusId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
        public int? RevUserId { get; set; }
        public DateTime? RevDateTime { get; set; }
    }

}
