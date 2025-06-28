using Web.Application.Common.Mappings;
using Web.Domain.Entities.Identity;

namespace Web.Application.Features.IdentityFeatures.Users.Queries
{
    public class UserGetByIdDto : UserDto, IMapFrom<User>
    {
		public List<int> RolesByUserList { get; set; }
	}
}
