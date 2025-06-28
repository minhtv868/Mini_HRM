using Web.Application.Common.Mappings;
using Web.Domain.Entities.Identity;

namespace Web.Application.Features.IdentityFeatures.Roles.Queries
{
    public class RoleGetByIdDto : RoleDto, IMapFrom<Role>
    {
		public List<int> SysFunctionsByRoleList { get; set; }
	}
}
