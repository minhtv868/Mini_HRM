using IC.Application.Common.Mappings;
using IC.Domain.Entities.Identity;

namespace IC.Application.Features.IdentityFeatures.Roles.Queries
{
    public class RoleGetByIdDto : RoleDto, IMapFrom<Role>
    {
		public List<int> SysFunctionsByRoleList { get; set; }
	}
}
