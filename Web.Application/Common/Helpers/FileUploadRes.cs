using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.Common.Helpers
{
    public class FileUploadRes
    {
        public string FileName { get; set; }
        public string PhysicalFilePath { get; set; }
        public string UrlFilePath { get; set; }
        public string MediaDomain { get; set; }
        public string RoutePrefix { get; set; }
        public string MediaFileName { get; set; }
        public string MediaName { get; set; }
        public string MediaUrl { get; set; }
        public byte? MediaTypeId { get; set; }
        public string OriginalFileName { get; set; }
        public int? FileSize { get; set; }
        public int? ImageWidth { get; set; }
        public int? ImageHeight { get; set; }
        public int? Duration { get; set; }
        public string Message { get; set; } = "OK";
    }
}
