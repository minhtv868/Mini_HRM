namespace WebJob.Models
{
	public class FileUploadReq
    {
        public FileUploadReq(string rootDir, string mediaRouteName)
        {
            RootDir = rootDir;
			MediaRouteName = mediaRouteName;
		}
		public FileUploadReq(string rootDir)
        {
            RootDir = rootDir;
        }

        public string MediaRouteName { get; set; }
        public string RootDir { get; set; }
		public string TempDir { get; set; } = "uploads\\temp";
        public string FileName { get; set; }
        public bool CreateImageThumbnail { get; set; } = false;
        public bool CreateWatermark { get; set; } = false;
        public bool GetImageSize { get; set; } = true;
        public List<ThumbImageSettings> ThumbImageSettings { get; set; }
    }
}
