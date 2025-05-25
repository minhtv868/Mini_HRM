using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IC.WebJob.Models
{
	public class AjaxResult : ActionResult
	{
		public string Id { get; set; }
		public object Data { get; set; }
		public bool Succeeded { get; set; }
		public List<string> Messages { get; set; }

		public string ReturnUrl { get; set; }

		public HttpStatusCode StatusCode;

		public AjaxResult()
		{
			StatusCode = HttpStatusCode.OK;
		}

		public override async Task ExecuteResultAsync(ActionContext context)
		{
			var responseData = new
			{
				Id = this.Id,
				Data = this.Data,
				Succeeded = this.Succeeded,
				Messages = this.Messages,
				ReturnUrl = this.ReturnUrl
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
