using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebJob.Models
{
	public class ExceptionAjaxResult : ActionResult
	{
		public string Messages { get; set; }

		public HttpStatusCode StatusCode;

		public ExceptionAjaxResult()
		{
			StatusCode = HttpStatusCode.OK;
		}

		public override async Task ExecuteResultAsync(ActionContext context)
		{
			var responseData = new
			{
				Messages = this.Messages
			};

			HttpResponse response = context.HttpContext.Response;
			response.Clear();
			response.ContentType = "application/json";
			response.StatusCode = (int)this.StatusCode;
			await response.WriteAsJsonAsync(responseData);
			await base.ExecuteResultAsync(context);
		}
	}
}
