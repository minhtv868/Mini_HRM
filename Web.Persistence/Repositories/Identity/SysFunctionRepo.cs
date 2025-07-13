using Web.Application.Interfaces.Repositories;
using Web.Application.Interfaces.Repositories.Identity;
using Web.Domain.Entities.Identity;
using Web.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Web.Persistence.Repositories.Identity
{
    public class SysFunctionRepo : ISysFunctionRepo
    {
        private readonly FinanceDbContext _dbContext;
        private readonly IGenericRepository<SysFunction> _repository;
        private int treeOrder = 0;
        public SysFunctionRepo(FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
            _repository = new GenericRepository<SysFunction>(dbContext);
        }

        public async Task UpdateTreeOrder()
        {
            var itemsList = await _repository.Entities.AsNoTracking()
                .ToListAsync();
            HashSet<int> visited = new HashSet<int>();
            await UpdateTreeOrder(itemsList, 0, 1, visited);
        }

        private async Task UpdateTreeOrder(List<SysFunction> itemsList, int parentId, byte treeLevel, HashSet<int> visited)
        {
            var entities = itemsList.FindAll(x => x.ParentItemId == parentId).OrderBy(x => x.DisplayOrder).ToList();
            foreach (var entity in entities)
            {
                if (visited.Contains(entity.Id)) continue;
                treeOrder++;
                entity.TreeLevel = treeLevel;
                entity.TreeOrder = treeOrder;
                entity.HasChild = itemsList.Any(x => x.ParentItemId == entity.Id);

                await _repository.UpdateAsync(entity.Id, entity);
                await _dbContext.SaveChangesAsync(default);
                visited.Add(entity.Id);

                await UpdateTreeOrder(itemsList, entity.Id, (byte)(treeLevel + 1), visited);
            }
        }

        //private static List<Dto> GetItemsByParentId(List<Dto> list, int parentId)
        //{
        //    var result = list.FindAll(p => p.ParentItemId == parentId);
        //    if (result == null) result = new List<Dto>();
        //    return result;
        //}

        //private async Task UpdateSQL(Dto item)
        //{
        //    var commandText = string.Format("UPDATE SysFunctions SET TreeOrder={0}, TreeLevel={1} WHERE Id={2}", item.TreeOrder, item.TreeLevel, item.Id);
        //    await _dbContext.Database.ExecuteSqlRawAsync(commandText);
        //}

        public async Task<List<SysFunction>> GetMenuByUserId(int userId)
        {
            string query = string.Format("SELECT * FROM SysFunctions WHERE IsEnable=1 AND IsShow=1 AND Id IN (SELECT SysFunctionId FROM SysFunctionRoles WHERE RoleId IN (SELECT RoleId FROM UserRoles WHERE UserId={0})) ORDER BY DisplayOrder", userId);
            var items = await _repository.DbSet.FromSqlRaw(query).AsNoTracking().ToListAsync();
            return items;
        }

        public async Task<List<SysFunction>> GetMenuByUserName(string userName)
        {
            var userId = await _dbContext.Users.Where(x => x.UserName == userName).AsNoTracking().Select(x => x.Id).SingleOrDefaultAsync();
            return await GetMenuByUserId(userId);
        }

        public async Task<bool> UserHasPermissionOnUrl(string userName, string url, bool allowParentUrl = true)
        {
            var userId = await _dbContext.Users.Where(x => x.UserName == userName).AsNoTracking().Select(x => x.Id).SingleOrDefaultAsync();
            return await UserHasPermissionOnUrl(userId, url, allowParentUrl);
        }

        public async Task<bool> UserHasPermissionOnUrl(int userId, string url, bool allowParentUrl = true)
        {
            string queryUrlInPermission = string.Format("SELECT id FROM SysFunctions WHERE IsEnable=1 AND Id IN (SELECT SysFunctionId FROM SysFunctionRoles WHERE RoleId IN (SELECT RoleId FROM UserRoles WHERE UserId={0})) AND Url='{1}'", userId, url);
            string queryUrlNotInPermission = string.Format("SELECT id FROM SysFunctions WHERE IsEnable=1 AND Id IN (SELECT SysFunctionId FROM SysFunctionRoles WHERE RoleId NOT IN (SELECT RoleId FROM UserRoles WHERE UserId={0})) AND Url='{1}'", userId, url);

            var result = await _repository.DbSet.FromSqlRaw(queryUrlInPermission).AsNoTracking().AnyAsync();
            if (result) return true;

            result = await _repository.DbSet.FromSqlRaw(queryUrlNotInPermission).AsNoTracking().AnyAsync();
            if (result) return false;

            if (allowParentUrl)
            {
                var parentUrl = "";
                var arrUrl = url.Split('/');
                if (arrUrl.Length > 1)
                {
                    for (int i = 0; i < arrUrl.Length - 1; i++)
                    {
                        if (arrUrl[i] != "") parentUrl = parentUrl + "/" + arrUrl[i];
                    }
                }
                if (parentUrl != "")
                {
                    string queryParentUrlInPermission = string.Format("SELECT id FROM SysFunctions WHERE IsEnable=1 AND Id IN (SELECT SysFunctionId FROM SysFunctionRoles WHERE RoleId IN (SELECT RoleId FROM UserRoles WHERE UserId={0})) AND Url='{1}'", userId, parentUrl);
                    result = await _repository.DbSet.FromSqlRaw(queryParentUrlInPermission).AsNoTracking().AnyAsync();
                    if (result) return true;
                }
            }

            return false;
        }

        private class Dto : SysFunction { }
    }
}