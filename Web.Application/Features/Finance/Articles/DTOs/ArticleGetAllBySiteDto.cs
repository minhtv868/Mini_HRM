using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Articles.DTOs
{
    public class ArticleGetAllBySiteDto : IMapFrom<Article>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? PublishTime { get; set; }
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
