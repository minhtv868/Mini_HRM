using Web.Domain.Entities.Identity;

namespace Web.Application.Interfaces.Repositories.Identity
{
    public interface IUserRepo
    {
        Task UpdateLastTimeLogin(string userName);
        Task UpdateLastTimeChangePass(string userName);
        Task<User> GetByUserNameAsync(string userName);
		Task<User> GetByIdAsync(int id);
		Task<List<User>> GetListUser(List<int> userIds); 
        Task<List<User>> GetAllUser();
    }
}
