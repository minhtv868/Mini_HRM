using IC.Domain.Entities;
using IC.Domain.Entities.Identity;

namespace IC.Application.Interfaces.Repositories.Identity
{
    public interface IRoleRepo
    {
        Task<List<Role>> GetAllAsync();
    }
}
