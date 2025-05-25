using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace IC.Application.Common.Helpers
{
    public static class EnumHelper
    {
		public static List<SelectListItem> GetEnumSelectList<T>() where T : Enum
		{
			return Enum.GetValues(typeof(T)).Cast<T>().Select(e => new SelectListItem
			{
				Value = e.ToString(),
				Text = GetDisplayName(e)
			}).ToList();
		}

		private static string GetDisplayName<T>(T enumValue) where T : Enum
		{
			var displayAttribute = enumValue.GetType()
				.GetField(enumValue.ToString())
				.GetCustomAttribute<DisplayAttribute>();

			return displayAttribute?.Name ?? enumValue.ToString();
		}
	}
}
