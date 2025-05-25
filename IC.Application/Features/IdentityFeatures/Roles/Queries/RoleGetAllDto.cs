using IC.Application.Common.Mappings;
using IC.Domain.Entities.Identity;

namespace IC.Application.Features.IdentityFeatures.Roles.Queries
{
    public class RoleGetAllDto : RoleDto, IMapFrom<Role>
    {
    }
}
