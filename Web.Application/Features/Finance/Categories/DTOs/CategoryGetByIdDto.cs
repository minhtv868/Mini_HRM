using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Categories.DTOs
{
    public class CategoryGetByIdDto : IMapFrom<Category>
    {
        public short CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }
        public string ShortName { get; set; }
        public string CategoryUrl { get; set; }
        public short? DataTypeId { get; set; }
        public short? SiteId { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKeyword { get; set; }
        public string CanonicalTag { get; set; }
        public string H1Tag { get; set; }
        public string SeoFooter { get; set; }
        public int? ParentCategoryId { get; set; }
        public byte? CategoryLevel { get; set; }
        public string ImagePath { get; set; }
        public int? DisplayOrder { get; set; }
        public int? TreeOrder { get; set; }
        public byte ReviewStatusId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
    }
}
