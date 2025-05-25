using Microsoft.AspNetCore.Identity;

namespace IC.Domain.Entities.Identity
{
    public class Role : IdentityRole<int>
    {
        public override int Id { get; set; }
        public string Description { get; set; }
    }
}
