using Web.Application.Common.Mappings;
using Web.Domain.Entities.Identity;

namespace Web.Application.Features.IdentityFeatures.Roles.Queries
{
    public class RoleGetAllDto : RoleDto, IMapFrom<Role>
    {
    }
}
