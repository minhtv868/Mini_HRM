namespace IC.WebJob.Models
{
    public class FileUploadRes
    {
		public string MediaDomain { get; set; }
		public string RoutePrefix { get; set; }
		public string MediaFileName { get; set; }
		public string MediaName { get; set; }
		public string MediaUrl { get; set; }
		public byte? MediaTypeId { get; set; }
		public string OriginalFileName { get; set; }
        public string PhysicalFilePath { get; set; }
        public int? FileSize { get; set; }
        public int? ImageWidth { get; set; }
        public int? ImageHeight { get; set; }
        public int? Duration { get; set; }
		public string Message { get; set; } = "OK";
    }
}
