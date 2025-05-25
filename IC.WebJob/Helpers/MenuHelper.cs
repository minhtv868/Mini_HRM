using IC.Application.Features.IdentityFeatures.SysFunctions.Queries;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IC.WebJob.Helpers
{
    public static class MenuHelper
    {
        public static List<SysFunctionGetMenuByUserDto> BuildMenuTree(List<SysFunctionGetMenuByUserDto> list)
        {
            List<SysFunctionGetMenuByUserDto> items = new List<SysFunctionGetMenuByUserDto>();
            foreach (var item in list.FindAll(p => p.ParentItemId == 0))
            {
                item.FunctionDesc = "<b>" + item.FunctionDesc + "</b>";
                items.Add(item);
                foreach (var item1 in GetItemsByParentId(list, item.Id))
                {
                    item1.FunctionDesc = "&nbsp;&nbsp;" + item1.FunctionDesc;
                    items.Add(item1);
                    foreach (var item2 in GetItemsByParentId(list, item1.Id))
                    {
                        item2.FunctionDesc = "&nbsp;&nbsp;&nbsp;&nbsp;" + item2.FunctionDesc;
                        items.Add(item2);

                    }
                }
            }
            return items;
        }

        public static List<SelectListItem> BuildDropDownParent(List<SysFunctionGetMenuByUserDto> list, int editId)
        {
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem() { Text = "...", Value = "0" });

            foreach (var item in list)
            {
                if (item.Id != editId) selectListItems.Add(new SelectListItem() { Text = item.GetNameByLevel(true), Value = item.Id.ToString() });
            }

            return selectListItems;
        }

        public static List<SysFunctionGetMenuByUserDto> GetRootItems(List<SysFunctionGetMenuByUserDto> list)
        {
            return list.FindAll(p => p.IsShow == true && p.ParentItemId == 0);
        }

        public static List<SysFunctionGetMenuByUserDto> GetItemsByParentId(List<SysFunctionGetMenuByUserDto> list, int ParentId)
        {
            var result = list.FindAll(p => p.IsShow == true && p.ParentItemId == ParentId);
            if (result == null) result = new List<SysFunctionGetMenuByUserDto>();
            return result;
        }

		public static List<SysFunctionGetMenuByUserDto> GetFavoriteFunctionsByParentId(List<SysFunctionGetMenuByUserDto> list, int ParentId)
		{
			var result = list.FindAll(p => p.IsShow == true && p.ParentItemId == ParentId && p.IsFavorite);
			if (result == null) result = new List<SysFunctionGetMenuByUserDto>();
			return result;
		}

		private static bool IsHasChild(List<SysFunctionGetMenuByUserDto> list, int ItemId)
        {
            return list.Any(x => x.ParentItemId == ItemId);
        }

        private static void SetHasChild(List<SysFunctionGetMenuByUserDto> list, SysFunctionGetMenuByUserDto itemActive)
        {
            foreach (var item in list)
            {
                item.HasChild = IsHasChild(list, item.Id);
                //reset active and open menu
                if (itemActive != null)
                {
                    item.CssMenuOpen = "";
                    item.CssMenuActive = "";
                }
                if (string.IsNullOrEmpty(item.IconPath))
                {
                    if (item.ParentItemId == 0) item.IconPath = "fas fa-flag";
                    else item.IconPath = "far fa-dot-circle";
                }
            }
        }

        public static void SetMenuActive(List<SysFunctionGetMenuByUserDto> list, string ItemUrl)
        {
            var itemActive = list.FindAll(x => ItemUrl.Contains(x.Url, StringComparison.OrdinalIgnoreCase)).OrderByDescending(o => o.Url).FirstOrDefault();
            SetHasChild(list, itemActive);
            if (itemActive != null)
            {
                itemActive.CssMenuOpen = "show";
                itemActive.CssMenuActive = "active";

                //Set active parent
                if (itemActive.ParentItemId > 0)
                {
                    var itemParent = list.First(x => x.Id == itemActive.ParentItemId);
                    if (itemParent != null)
                    {
                        itemParent.CssMenuOpen = "show";
                        itemParent.CssMenuActive = "active";

                        //Set active grand parent
                        if (itemParent.ParentItemId > 0)
                        {
                            var itemGrandParent = list.First(x => x.Id == itemParent.ParentItemId);
                            if (itemGrandParent != null)
                            {
                                itemGrandParent.CssMenuOpen = "show";
                                itemGrandParent.CssMenuActive = "active";
                            }
                        }
                    }
                }
            }
        }
    }
}
