using IC.Application.Common.Mappings;
using IC.Domain.Entities.Identity;

namespace IC.Application.Features.IdentityFeatures.Users.Queries
{
    public class UserGetByIdDto : UserDto, IMapFrom<User>
    {
		public List<int> RolesByUserList { get; set; }
	}
}
