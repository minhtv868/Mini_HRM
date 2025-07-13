using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Categories.DTOs
{
    public class CategoryGetPageDto : IMapFrom<Category>
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }
        public short? DataTypeId { get; set; }
        public short? FeatureGroupId { get; set; }
        public short? SiteId { get; set; }
        public int? DisplayOrder { get; set; }
        public int? TreeOrder { get; set; }
        public byte ReviewStatusId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
        public byte? CategoryLevel { get; set; }

        //
        public string CrUserName { get; set; }
        public string DataTypeName { get; set; }
        public string ReviewStatusName { get; set; }
        public string FeatureGroupName { get; set; }
    }
}
