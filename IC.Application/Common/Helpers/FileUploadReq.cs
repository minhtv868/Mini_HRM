using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC.Application.Common.Helpers
{

    public class FileUploadReq
    {
        public FileUploadReq()
        {
        }
        public FileUploadReq(string rootDir)
        {
            RootDir = rootDir;
        }

        public FileUploadReq(string rootDir, string uploadDir) : this(rootDir)
        {
            UploadDir = uploadDir;
        }

        public FileUploadReq(string rootDir, string uploadDir, bool createImageThumbnail) : this(rootDir, uploadDir)
        {
            CreateImageThumbnail = createImageThumbnail;
        }

        public FileUploadReq(bool usingServiceApi, string rootDir, string serviceApiUrl, string serviceApiKey) : this(rootDir)
        {
            UsingUploadApi = usingServiceApi;
            UploadApiUrl = serviceApiUrl;
            UploadApiKey = serviceApiKey;
        }

        public string RootDir { get; set; }
        public string UploadDir { get; set; } = "Uploads";
        public string TempDir { get; set; } = "Temp";
        public string FileName { get; set; }
        public string FilePathOld { get; set; }
        public bool UsingDirByFileType { get; set; } = true;
        public bool IgnoreUploadDirInUrlFilePath { get; set; } = false;
        public bool CreateImageThumbnail { get; set; } = false;
        public bool CreateWatermark { get; set; } = false;
        public string ImageWaterMark { get; set; }
        public bool GetImageSize { get; set; } = true;
        public bool UsingUploadApi { get; set; } = false;
        public string UploadApiUrl { get; set; }
        public string CopyFileApiUrl { get; set; }
        public string UploadApiKey { get; set; }
        public List<ThumbImageSettings> ThumbImageSettings { get; set; }
        public FacebookImageSettings FacebookImageSettings { get; set; }
        public AmpImageSettings AmpImageSettings { get; set; }
    }
}
