using Web.Domain.Entities;
using Web.Domain.Entities.Identity;

namespace Web.Application.Interfaces.Repositories.Identity
{
    public interface IRoleRepo
    {
        Task<List<Role>> GetAllAsync();
    }
}
