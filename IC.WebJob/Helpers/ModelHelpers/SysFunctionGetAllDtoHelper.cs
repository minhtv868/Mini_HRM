using IC.Application.Features.IdentityFeatures.SysFunctions.Queries;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IC.WebJob.Helpers.ModelHelpers
{
    public static class SysFunctionGetAllDtoHelper
    {
        public static List<SysFunctionGetAllDto> BuildMenuTree(List<SysFunctionGetAllDto> list)
        {
            List<SysFunctionGetAllDto> items = new List<SysFunctionGetAllDto>();
            foreach (var item in list.FindAll(p => p.ParentItemId == 0))
            {
                item.FunctionName = "<b>" + item.FunctionName + "</b>";
                items.Add(item);
                foreach (var item1 in GetItemsByParentId(list, item.Id))
                {
                    item1.FunctionName = "&nbsp;&nbsp;" + item1.FunctionName;
                    items.Add(item1);
                    foreach (var item2 in GetItemsByParentId(list, item1.Id))
                    {
                        item2.FunctionName = "&nbsp;&nbsp;&nbsp;&nbsp;" + item2.FunctionName;
                        items.Add(item2);

                    }
                }
            }
            return items;
        }

        //public static PaginatedList<GetSysFunctionGetAllDto> BuildMenuTree(PaginatedList<GetSysFunctionGetAllDto> list)
        //{
        //    List<SysMenuItem> items = new List<SysMenuItem>();
        //    foreach (var item in list.FindAll(p => p.ParentItemId == 0))
        //    {
        //        item.FunctionName = "<b>" + item.FunctionName + "</b>";
        //        items.Add(item);
        //        foreach (var item1 in GetItemsByParentId(list, item.Id))
        //        {
        //            item1.FunctionName = "&nbsp;&nbsp;" + item1.FunctionName;
        //            items.Add(item1);
        //            foreach (var item2 in GetItemsByParentId(list, item1.Id))
        //            {
        //                item2.FunctionName = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + item2.FunctionName;
        //                items.Add(item2);

        //            }
        //        }
        //    }
        //    return PaginatedList<SysMenuItem>.Create(items, items.Count, list.PageIndex, 1000, list.Url);
        //}

        public static List<SelectListItem> BuildDropDownParent(List<SysFunctionGetAllDto> list, int editId)
        {
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem() { Text = "...", Value = "0" });

            foreach (var item in list)
            {
                if (item.Id != editId) selectListItems.Add(new SelectListItem() { Text = item.GetNameByLevel(true), Value = item.Id.ToString() });
            }

            return selectListItems;
        }

        public static List<SysFunctionGetAllDto> GetRootItems(List<SysFunctionGetAllDto> list)
        {
            return list.FindAll(p => p.IsShow == true && p.ParentItemId == 0);
        }

        public static List<SysFunctionGetAllDto> GetItemsByParentId(List<SysFunctionGetAllDto> list, int ParentId)
        {
            var result = list.FindAll(p => p.IsShow == true && p.ParentItemId == ParentId);
            if (result == null) result = new List<SysFunctionGetAllDto>();
            return result;
        }

        private static bool IsHasChild(List<SysFunctionGetAllDto> list, int ItemId)
        {
            return list.Any(x => x.ParentItemId == ItemId);
        }

        private static void SetHasChild(List<SysFunctionGetAllDto> list, SysFunctionGetAllDto itemActive)
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
                    if (item.ParentItemId == 0) item.IconPath = "far fa-circle nav-icon";
                    else item.IconPath = "far fa-dot-circle nav-icon";
                }
            }
        }

        public static void SetCssMenuActive(List<SysFunctionGetAllDto> list, string Url)
        {
            var itemActive = list.FindAll(x => Url.Contains(x.Url, StringComparison.OrdinalIgnoreCase)).OrderByDescending(o => o.Url).FirstOrDefault();
            if (itemActive != null)
            {
                SetHasChild(list, itemActive);
                itemActive.CssMenuOpen = "menu-open";
                itemActive.CssMenuActive = "active";

                //Set active parent
                if (itemActive.ParentItemId > 0)
                {
                    var itemParent = list.First(x => x.Id == itemActive.ParentItemId);
                    if (itemParent != null)
                    {
                        itemParent.CssMenuOpen = "menu-open";
                        itemParent.CssMenuActive = "active";

                        //Set active grand parent
                        if (itemParent.ParentItemId > 0)
                        {
                            var itemGrandParent = list.First(x => x.Id == itemParent.ParentItemId);
                            if (itemGrandParent != null)
                            {
                                itemGrandParent.CssMenuOpen = "menu-open";
                                itemGrandParent.CssMenuActive = "active";
                            }
                        }
                    }
                }
            }
        }
    }
}
