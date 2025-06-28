using Web.Application.Common.Mappings;
using Web.Application.Features.IdentityFeatures.Roles.Queries;
using Web.Domain.Entities.Identity;

namespace Web.Application.Features.IdentityFeatures.Users.Queries
{
    public class UserGetPageDto : UserDto, IMapFrom<User>
    {
        public List<RoleDto> Roles { get; set; }
        
        public string GetRoleString()
        {
            string roles = "";
            if (Roles != null)
            {
                foreach (var item in Roles)
                {
                    if (roles != "") roles += " ";
                    roles += "<span class=\"badge bg-success\">" + item.Name + "</span>";
                }
            }
            return roles;
        }
    }
}
