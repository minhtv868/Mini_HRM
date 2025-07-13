using Web.Domain.Entities.Finance;

namespace Webs.Application.Finance.Categories.Helper
{
    public class SearchTree
    {
        public static List<int> SearchTreeOrder(List<Category> Categorys, int? CategoryId)
        {
            var result = new List<int>();
            var visited = new HashSet<int>();
            var parentSearch = Categorys.FirstOrDefault(x => x.CategoryId == CategoryId);
            if (parentSearch != null)
            {
                result.Add(parentSearch.CategoryId);
                visited.Add(parentSearch.CategoryId);
                UpdateTreeOrderRecursive(Categorys, parentSearch.CategoryId, parentSearch.DisplayOrder ?? 0, result, visited);
            }
            return result;
        }

        public static int UpdateTreeOrderRecursive(List<Category> itemsList, int? parentId, int currentOrder, List<int> result, HashSet<int> visited)
        {
            var entities = itemsList
                .Where(x => x.ParentCategoryId == parentId)
                .OrderBy(x => x.DisplayOrder)
                .ToList();

            foreach (var entity in entities)
            {
                if (visited.Contains(entity.CategoryId))
                {
                    continue;
                }

                currentOrder++;
                result.Add(entity.CategoryId);
                visited.Add(entity.CategoryId);
                currentOrder = UpdateTreeOrderRecursive(itemsList, entity.CategoryId, currentOrder, result, visited);
            }
            return currentOrder;
        }

    }
}
