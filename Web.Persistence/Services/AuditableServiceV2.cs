using Web.Application.DTOs;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Identity;
using Web.Shared.Helpers;

namespace Web.Persistence.Services
{
    public class AuditableServicev2(IUserRepo userRepo) : IAuditableService
    {
        public async Task UpdateAuditableInfoAsync<T>(List<T> data) where T : IAuditable
        {
            if (!data.IsAny())
                return;

            List<int> userIdsList = new();

			if (data.Any(x => x.GetType().GetProperty("CrUserId") != null && x.CrUserId.HasValue) ||
				data.Any(x => x.GetType().GetProperty("UpdUserId") != null && x.UpdUserId.HasValue))
			{
				userIdsList = data.Where(x => x.CrUserId.HasValue)
					.Select(x => x.CrUserId.Value)
					.Union(data.Where(x => x.UpdUserId.HasValue)
					.Select(x => x.UpdUserId.Value))
					.Distinct()
					.ToList();
			}
			else if (data.Any(x => x.GetType().GetProperty("CreatedBy") != null && x.CreatedBy.HasValue) ||
					 data.Any(x => x.GetType().GetProperty("UpdatedBy") != null && x.UpdatedBy.HasValue))
			{
				userIdsList = data.Where(x => x.CreatedBy.HasValue)
					.Select(x => x.CreatedBy.Value)
					.Union(data.Where(x => x.UpdatedBy.HasValue)
					.Select(x => x.UpdatedBy.Value))
					.Distinct()
					.ToList();
			}

			if (userIdsList.IsAny())
            {
                var usersList = await userRepo.GetListUser(userIdsList);

                foreach (var item in data)
                {
                    item.AuditableInfo = new AuditableInfoDto
                    {
                        CreatedByUserName = usersList.FirstOrDefault(x => x.Id == item.CrUserId)?.UserName,
                        UpdatedByUserName = usersList.FirstOrDefault(x => x.Id == item.UpdUserId)?.UserName
                    };
                }
            }
        }
    }
}
