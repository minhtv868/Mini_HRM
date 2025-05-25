using IC.Application.Common.Mappings;
using IC.Domain.Entities.Identity;

namespace IC.Application.Features.IdentityFeatures.Roles.Queries
{
    public class RoleDto : IMapFrom<Role>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
