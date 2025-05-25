using IC.Application.DTOs;
using IC.Application.Interfaces;
using IC.Application.Interfaces.Repositories.Identity;
using IC.Shared.Helpers;

namespace IC.Persistence.Services
{
    public class AuditableService(IUserRepo userRepo) : IAuditableService
    {
        public async Task UpdateAuditableInfoAsync<T>(List<T> data) where T : IAuditable
        {
            if (!data.IsAny())
                return;

            var userIdsList = data.Where(x => x.CreatedBy.HasValue).Select(x => x.CreatedBy.Value)
                .Union(data.Where(x => x.UpdatedBy.HasValue).Select(x => x.UpdatedBy.Value)).Distinct().ToList();

            if (userIdsList.IsAny())
            {
                var usersList = await userRepo.GetListUser(userIdsList);

                foreach (var item in data)
                {
                    item.AuditableInfo = new AuditableInfoDto
                    {
                        CreatedByUserName = usersList.FirstOrDefault(x => x.Id == item.CreatedBy)?.UserName,
                        UpdatedByUserName = usersList.FirstOrDefault(x => x.Id == item.UpdatedBy)?.UserName
                    };
                }
            }
        }
    }
}
