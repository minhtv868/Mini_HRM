namespace WebJob.Models
{
	public class PagingInput
	{
		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 1;
		public int TotalPages { get; set; } = 0;

		public string Handler { get; set; } = string.Empty;
		public string ElementUpdate { get; set; } = string.Empty;
		public string AjaxOnSuccess { get; set; } = string.Empty;
		public PagingInput(int page, int totalPage)
		{
			TotalPages = totalPage;
			Page = page > 1 ? page : 1;
		}
		public PagingInput(int page, int pageSize, int totalPage)
		{
			Page = page > 1 ? page : 1;
			PageSize = pageSize;
			TotalPages = totalPage;
		}

        public PagingInput(int page, int pageSize, int totalPage, string elementUpdate, string ajaxOnSuccess, string handler)
        {
            Page = page > 1 ? page : 1;
            PageSize = pageSize;
            TotalPages = totalPage;
			ElementUpdate = elementUpdate;
			AjaxOnSuccess = ajaxOnSuccess;
			Handler = handler;
        }
    }
}
