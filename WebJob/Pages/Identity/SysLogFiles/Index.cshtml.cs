using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebJob.Pages.Identity.SysLogFiles
{
	public class IndexModel : PageModel
    {
		private readonly IWebHostEnvironment hostEnvironment;
		public IEnumerable<FileInfo> Files;

		public IndexModel(IWebHostEnvironment hostEnvironment)
		{
			this.hostEnvironment = hostEnvironment;
		}

		public void OnGet()
        {
			DirectoryInfo dir = new DirectoryInfo(Path.Combine(hostEnvironment.ContentRootPath, "Logs"));
			if(!dir.Exists )
			{
				Directory.CreateDirectory(dir.FullName);
			}
			Files = dir.GetFiles().ToList().OrderByDescending(x => x.CreationTime).Take(100);
		}
    }
}
