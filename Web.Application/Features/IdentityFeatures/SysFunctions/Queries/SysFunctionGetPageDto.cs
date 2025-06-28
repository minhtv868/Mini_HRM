using Web.Application.Features.IdentityFeatures.Roles.Queries;

namespace Web.Application.Features.IdentityFeatures.SysFunctions.Queries
{
    public class SysFunctionGetPageDto : SysFunctionDto
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
