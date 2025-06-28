using System.Text.RegularExpressions;

namespace WebJob.Helpers
{
	public static class PagingHelper
	{
		public static string PaginationLink(this string currentUrl, int page = 1)
		{
			if (!string.IsNullOrEmpty(currentUrl))
			{
				currentUrl = Regex.Replace(currentUrl, @"[?|&]page=[0-9]+", string.Empty);

				return string.Format("{0}{1}{2}", currentUrl, currentUrl.Contains("?") ? "&page=" : "?page=", page);
			}

			return string.Empty;
		}
	}
}
