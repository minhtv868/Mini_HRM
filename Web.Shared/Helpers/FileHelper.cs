using System.Globalization;
using System.Text;

namespace Web.Shared.Helpers
{
    public static class FileHelper
    {
        private static readonly Dictionary<string, List<byte[]>> FileSignatures = new Dictionary<string, List<byte[]>>
        {
            { "image/jpeg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 } } }, // JPEG
            { "image/png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47 } } }, // PNG
            { "image/gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } }, // GIF
            { "image/bmp", new List<byte[]> { new byte[] { 0x42, 0x4D } } }, // BMP
            { "image/webp", new List<byte[]> { new byte[] { 0x52, 0x49, 0x46, 0x46, 0x57, 0x45, 0x42, 0x50 } } }, // WebP
            { "image/tiff", new List<byte[]> { new byte[] { 0x49, 0x49, 0x2A, 0x00 }, new byte[] { 0x4D, 0x4D, 0x00, 0x2A } } }, // TIFF
            { "image/svg+xml", new List<byte[]> { new byte[] { 0x3C, 0x3F, 0x78, 0x6D } } }, // SVG
            { "image/vnd.microsoft.icon", new List<byte[]> { new byte[] { 0x00, 0x00, 0x01, 0x00 } } }, // ICO
        
            // Microsoft Word
            { "application/msword", new List<byte[]> { new byte[] { 0xD0, 0xCF, 0x11, 0xE0 } } }, // DOC
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 } } }, // DOCX

            // Excel
            { "application/vnd.ms-excel", new List<byte[]> { new byte[] { 0xD0, 0xCF, 0x11, 0xE0 } } }, // XLS
            { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 } } }, // XLSX

            // PDF
            { "application/pdf", new List<byte[]> { new byte[] { 0x25, 0x50, 0x44, 0x46 } } },

            // File nén
            { "application/zip", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }, // ZIP
            { "application/x-rar-compressed", new List<byte[]> { new byte[] { 0x52, 0x61, 0x72, 0x21 } } }, // RAR
        };

        private static readonly Dictionary<string, List<byte[]>> RouterTypeSignatures = new()
        {
            { "Images/Original", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 }, // JPEG
                    new byte[] { 0x89, 0x50, 0x4E, 0x47 }, // PNG
                    new byte[] { 0x47, 0x49, 0x46, 0x38 }, // GIF
                    new byte[] { 0x42, 0x4D }, // BMP
                    new byte[] { 0x52, 0x49, 0x46, 0x46, 0x57, 0x45, 0x42, 0x50 }, // WebP
                    new byte[] { 0x49, 0x49, 0x2A, 0x00 }, new byte[] { 0x4D, 0x4D, 0x00, 0x2A }, // TIFF
                    new byte[] { 0x3C, 0x3F, 0x78, 0x6D }, // SVG
                    new byte[] { 0x00, 0x00, 0x01, 0x00 }  // ICO
                }
            },
            { "Others", new List<byte[]>
                {
                    new byte[] { 0xD0, 0xCF, 0x11, 0xE0 }, // DOC
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 }, // DOCX

                    new byte[] { 0xD0, 0xCF, 0x11, 0xE0 }, // XLS
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 }, // XLSX
                   
                    new byte[] { 0x25, 0x50, 0x44, 0x46 }, //PDF

                    new byte[] { 0x50, 0x4B, 0x03, 0x04 }, // ZIP
                    new byte[] { 0x52, 0x61, 0x72, 0x21 }, // RAR
                }
            }
        };

        public static async Task<string> GetRouterTypeByFileTypeAsync(string fileName)
        {
            string resultVar = "Others";

            try
            {
                byte[] header = await ReadFileHeaderAsync(fileName, 4).ConfigureAwait(false); //đọc 4 byte đầu tiên

                foreach (var fileType in RouterTypeSignatures)
                {
                    foreach (var signature in fileType.Value)
                    {
                        if (StartsWith(header, signature))
                        {
                            resultVar = fileType.Key;

                            return resultVar;
                        }
                    }
                }
            }
            catch
            {
            }

            return resultVar;
        }

        public static async Task<string> GetRouterTypeByFileTypeAsync(Stream fileStream)
        {
            string result = "Others";

            try
            {
                byte[] header = await ReadFileHeaderAsync(fileStream, 4).ConfigureAwait(false);

                foreach (var fileType in RouterTypeSignatures)
                {
                    foreach (var signature in fileType.Value)
                    {
                        if (StartsWith(header, signature))
                        {
                            result = fileType.Key;

                            return result;
                        }
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        public static async Task<string> GetFileTypeAsync(string filePath)
        {
            byte[] fileBytes;

            try
            {
                fileBytes = await ReadFileHeaderAsync(filePath).ConfigureAwait(false);
            }
            catch
            {
                return "application/octet-stream";
            }

            string fileType = DetectFileType(fileBytes);

            return fileType;
        }

        public static async Task<string> GetFileTypeAsync(Stream fileStream)
        {
            string resultVar = string.Empty;

            try
            {
                byte[] header = await ReadFileHeaderAsync(fileStream, 4).ConfigureAwait(false);

                foreach (var fileType in RouterTypeSignatures)
                {
                    foreach (var signature in fileType.Value)
                    {
                        if (StartsWith(header, signature))
                        {
                            resultVar = fileType.Key;

                            return resultVar;
                        }
                    }
                }
            }
            catch
            {
            }

            return resultVar;
        }

        private static async Task<byte[]> ReadFileHeaderAsync(string filePath, int headerSize = 4096)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[headerSize];

                await stream.ReadAsync(buffer, 0, headerSize).ConfigureAwait(false);

                return buffer;
            }
        }

        private static async Task<byte[]> ReadFileHeaderAsync(Stream fileStream, int headerSize = 4096)
        {
            byte[] buffer = new byte[headerSize];

            int totalBytesRead = 0;

            while (totalBytesRead < headerSize)
            {
                int bytesRead = await fileStream.ReadAsync(buffer, totalBytesRead, headerSize - totalBytesRead).ConfigureAwait(false);
                
                if (bytesRead == 0)
                    throw new EndOfStreamException("Unexpected end of stream");

                totalBytesRead += bytesRead;
            }

            return buffer;
        }

        public static async Task<byte[]> ReadFileAsync(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[stream.Length];

                await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

                return buffer;
            }
        }

        public static async Task<byte[]> ReadStreamAsync(Stream inputStream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await inputStream.CopyToAsync(memoryStream).ConfigureAwait(false);
                return memoryStream.ToArray();
            }
        }

        private static string DetectFileType(byte[] fileBytes)
        {
            foreach (var fileType in FileSignatures)
            {
                foreach (var signature in fileType.Value)
                {
                    if (StartsWith(fileBytes, signature))
                    {
                        return fileType.Key;
                    }
                }
            }

            return "application/octet-stream";
        }

        private static bool StartsWith(byte[] source, byte[] pattern)
        {
            if (pattern.Length > source.Length)
                return false;

            for (int i = 0; i < pattern.Length; i++)
            {
                if (source[i] != pattern[i])
                    return false;
            }

            return true;
        }

        public static string GetFileNameSanitize(string fileName)
        {
            string resultVar = fileName;

            if (!string.IsNullOrWhiteSpace(resultVar))
            {
                resultVar = GetSlug(resultVar);

                if (resultVar.LastIndexOf("\\") > 0)
                {
                    resultVar = resultVar.Substring(resultVar.LastIndexOf("\\") + 1);
                }

                resultVar = resultVar.Insert(resultVar.LastIndexOf("."), "_" + DateTime.Now.ToString("HHmmss"));

                return resultVar.ToLower();
            }

            return string.Empty;
        }

        private static string GetSlug(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormKD);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark && !char.IsWhiteSpace(c) && c != '-')
                {
                    stringBuilder.Append(c);
                }
                else if (c == 'Đ' || c == 'đ')
                {
                    stringBuilder.Append("d");
                }
                else if (char.IsWhiteSpace(c))
                {
                    stringBuilder.Append("-");
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormKC).ToLower();
        }
        public static string GetMimeType(string fileName)
        {
            string contentType = "application/octet-stream";
            try
            {
                fileName = fileName.ToLower();
                if(fileName.EndsWith("jpeg") || fileName.EndsWith("jpg"))
                {
                    contentType = "image/jpeg";
                }
                else if (fileName.EndsWith("png"))
                {
                    contentType = "image/png";
                }
                else if (fileName.EndsWith("gif"))
                {
                    contentType = "image/gif";
                }
                else if (fileName.EndsWith("bmp"))
                {
                    contentType = "image/bmp";
                }
                else if (fileName.EndsWith("tiff"))
                {
                    contentType = "image/tiff";
                }
                else if (fileName.EndsWith("svg"))
                {
                    contentType = "image/svg+xml";
                }
                else if (fileName.EndsWith("icon") || fileName.EndsWith("ico"))
                {
                    contentType = "image/vnd.microsoft.icon";
                }
                else if (fileName.EndsWith("doc") || fileName.EndsWith("docx"))
                {
                    contentType = "application/msword";
                }
                else if (fileName.EndsWith("xls") || fileName.EndsWith("xlsx"))
                {
                    contentType = "application/application/vnd.ms-excel";
                }
                else if (fileName.EndsWith("pdf"))
                {
                    contentType = "application/pdf";
                }
                else if (fileName.EndsWith("zip"))
                {
                    contentType = "application/zip";
                }
                else if (fileName.EndsWith("rar"))
                {
                    contentType = "application/x-rar-compressed";
                }
            }
            catch 
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
