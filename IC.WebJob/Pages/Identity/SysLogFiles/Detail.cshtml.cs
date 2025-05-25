using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IC.WebJob.Pages.Identity.SysLogFiles
{
    public class DetailModel : PageModel
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }
        public void OnGet(string filePath, string fileName)
        {
            FileName = fileName;
            FileContent = System.IO.File.ReadAllText(filePath);
        }
    }
}
