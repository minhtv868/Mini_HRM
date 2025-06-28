using Web.Application.Common.Mappings;
using Web.Domain.Entities.Identity;

namespace Web.Application.Features.IdentityFeatures.Users.Queries
{
	public class UserGetAllDto : IMapFrom<User>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string DisplayName => !string.IsNullOrWhiteSpace(FullName) ? FullName : UserName;
    }
}
