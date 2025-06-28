using FileTypeChecker.Abstracts;
using FileTypeChecker.Extensions;
using FileTypeChecker;
using Web.Application.Common.Constants;
using Web.Shared.Helpers;
using MediatR;
using NAudio.Wave;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using WebJob.Helpers;

namespace Web.Application.Common.Helpers
{
    public static class FileUploadHelper
    {
        private static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        private static readonly int MaxWidthAddLogo = 1000;
        public static bool IsImageFile(string FileName)
        {
            bool RetVal = false;
            if (string.IsNullOrEmpty(FileName)) return RetVal;

            string fileExt = Path.GetExtension(FileName).ToLower();
            string imageFile = ".jpg;.gif;.png;.bmp;.jpeg;.webp";
            if (imageFile.IndexOf(fileExt) >= 0) RetVal = true;
            return RetVal;
        }

        private static void DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name, ex.ToString());
            }
        }
        public static bool FileInUse(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    bool m_CanWrite = fs.CanWrite;
                }
                return false;
            }
            catch
            {
                return true;
            }
        }
        public static void CreateTextFile(string FilePath, string FileContent)
        {
            File.WriteAllText(FilePath, FileContent);
        }

        public static void GetImageFileInfo(FileUploadRes fileUploadInfo)
        {
            if (!IsImageFile(fileUploadInfo.PhysicalFilePath)) return;
            if (!OperatingSystem.IsWindows()) return;

            using (var fileStream = new FileStream(fileUploadInfo.PhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var img = Image.FromStream(fileStream, false, false))
                {
                    fileUploadInfo.ImageHeight = img.Height;
                    fileUploadInfo.ImageWidth = img.Width;
                }
            }
        }

        public static IFormFile ResizeImage(IFormFile imageFile, int maxDimension)
        {
            if (!OperatingSystem.IsWindows()) return imageFile;
            using (var inputStream = imageFile.OpenReadStream())
            {
                if (!inputStream.IsImage())
                    return imageFile;
                using (var outputStream = new MemoryStream())
                {
                    using (var inputImage = Image.FromStream(inputStream))
                    {
                        int originalWidth = inputImage.Width;
                        int originalHeight = inputImage.Height;

                        int newWidth = maxDimension;
                        int newHeight = maxDimension;

                        if (originalWidth > maxDimension || originalHeight > maxDimension)
                        {
                            if (originalWidth > originalHeight)
                            {
                                newHeight = (int)((double)originalHeight / originalWidth * maxDimension);
                            }
                            else
                            {
                                newWidth = (int)((double)originalWidth / originalHeight * maxDimension);
                            }
                            using (var resizedImage = new Bitmap(newWidth, newHeight))
                            {
                                using (var graphics = Graphics.FromImage(resizedImage))
                                {
                                    graphics.CompositingMode = CompositingMode.SourceCopy;
                                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                    graphics.DrawImage(inputImage, 0, 0, newWidth, newHeight);
                                }

                                resizedImage.Save(outputStream, ImageFormat.Jpeg);

                                outputStream.Seek(0, SeekOrigin.Begin);
                                var outputStreamCopy = new MemoryStream(outputStream.ToArray());

                                return new FormFile(outputStreamCopy, 0, outputStreamCopy.Length, null, Path.GetFileName(imageFile.FileName))
                                {
                                    Headers = new HeaderDictionary(),
                                    ContentType = "image/jpeg"
                                };
                            }
                        }
                        return imageFile;
                    }
                }
            }

        }
        public static string GetMimeType(string fileName)
        {
            string contentType = "";
            try
            {
                new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(ex.ToString(), ((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name);
            }
            return contentType;
        }
        public static int? GetDuration(string source)
        {
            int? duration = null;
            try
            {
                if (!string.IsNullOrEmpty(source))
                {
                    using (var reader = new AudioFileReader(source))
                    {
                        duration = (int)reader.TotalTime.TotalSeconds;
                    }
                }
            }
            catch (Exception ex)
            {
                //  LogHelper.WriteLog(((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name, ex.ToString());
            }

            return duration;
        }
        public static async Task<FileUploadRes> UploadToRemoteServerAsync(IFormFile fUpl, FileUploadReq uploadReq)
        {
            FileUploadRes uploadRes = new FileUploadRes();
            try
            {
                if (fUpl == null)
                {
                    uploadRes.Message = "Chưa chọn file";
                    return uploadRes;
                }

                var directoryPath = Path.Combine(uploadReq.RootDir, uploadReq.UploadDir, uploadReq.TempDir);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = StringHelper.RemoveSign4VietnameseString(Path.GetFileNameWithoutExtension(fUpl.FileName)).Replace(" ", "-");
                fileName = StringHelper.RemoveSignatureForURL(fileName);
                fileName = fileName + "-" + DateTime.Now.ToString("ddMMHHmmss");
                fileName = fileName + Path.GetExtension(fUpl.FileName);
                fileName = fileName.Replace(" ", "-");
                var filePath = Path.Combine(directoryPath, fileName).Replace("/", "\\");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    fUpl.CopyTo(stream);
                }

                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Headers.Add("ApiKey", uploadReq.UploadApiKey);

                        var stringContent = new StringContent(JsonConvert.SerializeObject(uploadReq));
                        stringContent.Headers.Add("Content-Disposition", "form-data; name=\"reqJson\"");
                        content.Add(stringContent, "json");

                        byte[] file = File.ReadAllBytes(filePath);
                        var byteArrayContent = new ByteArrayContent(file);
                        content.Add(byteArrayContent, "file", fUpl.FileName);

                        var result = await client.PostAsync(uploadReq.UploadApiUrl, content);
                        if (result.IsSuccessStatusCode)
                        {
                            uploadRes = StringHelper.Deserialize<FileUploadRes>(result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                        }
                    }
                }
                uploadRes.OriginalFileName = fUpl.FileName;

                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                uploadRes.Message = "Lỗi lưu file.";
                // LogHelper.WriteLog(((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name, ex.ToString());
            }
            return uploadRes;
        }

        private static async Task<FileUploadRes> ProcessUploadToRemoteServer(string fileName, string filePath, string uploadDir, FileUploadReq uploadReq)
        {
            FileUploadRes uploadRes = new FileUploadRes();
            uploadReq.UploadDir = uploadDir;
            uploadReq.FileName = fileName;
            try
            {
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Headers.Add("ApiKey", uploadReq.UploadApiKey);

                        var stringContent = new StringContent(JsonConvert.SerializeObject(uploadReq));
                        stringContent.Headers.Add("Content-Disposition", "form-data; name=\"reqJson\"");
                        content.Add(stringContent, "json");

                        byte[] file = File.ReadAllBytes(filePath);
                        var byteArrayContent = new ByteArrayContent(file);
                        content.Add(byteArrayContent, "file", fileName);

                        var result = await client.PostAsync(uploadReq.UploadApiUrl, content);
                        if (result.IsSuccessStatusCode)
                        {
                            uploadRes = StringHelper.Deserialize<FileUploadRes>(result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                        }
                        else
                        {
                            uploadRes.Message = $"Lỗi lưu file (remote server error {result.StatusCode}).";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                uploadRes.Message = "Lỗi lưu file (remote server).";
                // LogHelper.WriteLog(((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name, ex.ToString());
            }
            return uploadRes;
        }

        private static async Task<FileUploadRes> ProcessCopyFileOnRemoteServer(FileUploadReq uploadReq)
        {
            FileUploadRes uploadRes = new FileUploadRes();
            try
            {
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Headers.Add("ApiKey", uploadReq.UploadApiKey);

                        var stringContent = new StringContent(JsonConvert.SerializeObject(uploadReq));
                        stringContent.Headers.Add("Content-Disposition", "form-data; name=\"reqJson\"");
                        content.Add(stringContent, "json");

                        var result = await client.PostAsync(uploadReq.CopyFileApiUrl, content);
                        if (result.IsSuccessStatusCode)
                        {
                            uploadRes = StringHelper.Deserialize<FileUploadRes>(result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                        }
                        else
                        {
                            uploadRes.Message = $"Lỗi copy file (remote server error {result.StatusCode}).";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                uploadRes.Message = "Lỗi copy file (remote server).";
                //  LogHelper.WriteLog(((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name, ex.ToString());
            }
            return uploadRes;
        }
        public static FileUploadRes SaveFile(IFormFile fUpl, FileUploadReq uploadReq)
        {
            FileUploadRes uploadRes = new FileUploadRes();
            try
            {
                if (fUpl == null)
                {
                    uploadRes.Message = "Chưa chọn file";
                    return uploadRes;
                }
                var dirCurrentDay = DateTime.Now.ToString("yyyy/MM/dd");
                var virtualPath = Path.Combine(uploadReq.UploadDir, uploadReq.UsingDirByFileType ? GetDirByFileType(fUpl.FileName) : "", dirCurrentDay);
                var directoryPath = Path.Combine(uploadReq.RootDir, virtualPath);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                var fileName = StringHelper.convertToUnSign2(Path.GetFileNameWithoutExtension(fUpl.FileName)).Replace(" ", "-");
                fileName = StringHelper.RemoveSignatureForURL(fileName);
                fileName = fileName + "-" + DateTime.Now.ToString("ddMMHHmmss");
                fileName = fileName + Path.GetExtension(fUpl.FileName);
                fileName = fileName.Replace(" ", "-");
                var filePath = Path.Combine(directoryPath, fileName).Replace("/", "\\");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    fUpl.CopyTo(stream);
                }

                //Add Logo
                if (uploadReq.CreateWatermark)
                {
                    string origin_filePath = Path.Combine(directoryPath, "Original_" + fileName).Replace("/", "\\");
                    string logoWattermask = Path.Combine(uploadReq.RootDir, uploadReq.ImageWaterMark);

                    using (var m_ImageLogo = Image.FromFile(filePath))
                    {
                        ImageFormat imageFormat = new ImageFormat(m_ImageLogo.RawFormat.Guid);
                        m_ImageLogo.Dispose();
                        //string origin_filePath = Path.Combine(directoryPath, "Original_" + fileName).Replace("/", "\\");
                        //using (Image image = Image.FromFile(origin_filePath))
                        //using (Image watermarkImage = Image.FromFile(Path.Combine(uploadReq.RootDir, uploadReq.ImageWaterMark)))
                        //{
                        //    Bitmap m_facebookOrg = new Bitmap(image);
                        //    Bitmap m_Wattermask = new Bitmap(watermarkImage); 
                        //    Bitmap m_Bitmap = ImageHelper.WatermarkImage(m_facebookOrg, m_Wattermask, new Point(m_facebookOrg.Width - watermarkImage.Width - 36, m_facebookOrg.Height - m_Wattermask.Height - 36), 0);
                        //    m_Bitmap.Save(directoryPath + fileName, imageFormat);
                        //} 
                        using (Image watermarkImage = Image.FromFile(logoWattermask))
                        {
                            using (var image = Image.FromFile(filePath))
                            {
                                Bitmap m_facebookOrg = new Bitmap(image);
                                //300x34 
                                Bitmap m_Wattermask = new Bitmap(watermarkImage);
                                //1000x1000
                                Size size = m_Wattermask.Size;
                                if (m_facebookOrg.Width > MaxWidthAddLogo)
                                {
                                    size.Width = m_facebookOrg.Width * m_Wattermask.Width / MaxWidthAddLogo;
                                    size.Height = size.Width * m_Wattermask.Height / m_Wattermask.Width;
                                }

                                Bitmap m_Bitmap = ImageHelper.WatermarkImage2(m_facebookOrg, m_Wattermask, new Point(m_facebookOrg.Width - size.Width - 36, m_facebookOrg.Height - size.Height - 36), size, 0);
                                m_Bitmap.Save(origin_filePath, imageFormat);
                                m_facebookOrg.Dispose();
                                m_Wattermask.Dispose();
                                m_Bitmap.Dispose();
                            }
                        }
                    }
                    if (File.Exists(origin_filePath))
                    {
                        File.Delete(filePath);
                        File.Move(origin_filePath, filePath);
                    }
                }
                uploadRes.OriginalFileName = fUpl.FileName;
                uploadRes.FileName = fileName;
                uploadRes.UrlFilePath = uploadReq.IgnoreUploadDirInUrlFilePath ? Path.Combine(dirCurrentDay, fileName).Replace("\\", "/") : Path.Combine(virtualPath, fileName).Replace("\\", "/");
                uploadRes.PhysicalFilePath = filePath;
                uploadRes.FileSize = (int?)fUpl.Length;
                if (uploadReq.GetImageSize) GetImageFileInfo(uploadRes);

                if (uploadReq.UsingUploadApi)
                {
                    var apiResult = ProcessUploadToRemoteServer(fileName, filePath, virtualPath, uploadReq).GetAwaiter().GetResult();
                    if (apiResult != null && apiResult.Message != "OK")
                    {
                        uploadRes.Message = apiResult.Message;
                        return uploadRes;
                    }
                }
                if (IsImageFile(filePath))
                {

                    //Create thumb image
                    if (uploadReq.ThumbImageSettings != null)
                    {
                        foreach (var imageSetting in uploadReq.ThumbImageSettings)
                        {
                            if (imageSetting.CreateImage)
                            {
                                var virtualPathThumb = Path.Combine(imageSetting.PrefixPath, dirCurrentDay.Replace("/", "\\"));
                                string dirThumb = Path.Combine(uploadReq.RootDir, virtualPathThumb);
                                string fileThumb = Path.Combine(dirThumb, fileName);
                                if (!Directory.Exists(dirThumb))
                                {
                                    Directory.CreateDirectory(dirThumb);
                                }
                                CreateThumbnail(filePath, imageSetting.Width, imageSetting.Height).Save(fileThumb, ImageFormat.Jpeg);
                                if (uploadReq.UsingUploadApi)
                                {
                                    var apiResult = ProcessUploadToRemoteServer(fileName, fileThumb, virtualPathThumb, uploadReq).GetAwaiter().GetResult();
                                    if (apiResult.Message != "OK")
                                    {
                                        throw new Exception("Lỗi upload file thumb image (remote server)");
                                    }
                                    File.Delete(fileThumb);
                                }
                            }
                        }
                    }

                    //create amp image
                    if (uploadReq.AmpImageSettings != null && IsImageFile(fileName) && uploadReq.AmpImageSettings.CreateImage)
                    {
                        CreateAmpImage(fileName, filePath, uploadReq);
                    }

                    if (uploadReq.UsingUploadApi && uploadReq.FacebookImageSettings != null && !uploadReq.FacebookImageSettings.CreateImage)
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                uploadRes.Message = "Lỗi lưu file.";
                //    LogHelper.WriteLog(((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name, ex.ToString());
            }
            return uploadRes;
        }
        public static string GetDirByFileType(string FileName)
        {
            string RetVal = "Others";
            string fileExt = Path.GetExtension(FileName).ToLower();
            string imageFile = ".jpg;.jpeg;.gif;.png;.bmp";
            string videoFile = ".3gp;.mp4;.flv";
            string audioFile = ".mp3;.wav;.wma";
            string m3u8File = ".m3u8";

            if (imageFile.IndexOf(fileExt) >= 0) RetVal = "Images/Original";
            else if (videoFile.IndexOf(fileExt) >= 0) RetVal = "Videos";
            else if (audioFile.IndexOf(fileExt) >= 0) RetVal = "Audios";
            else if (m3u8File.IndexOf(fileExt) >= 0) RetVal = "M3u8";

            return RetVal;
        }
        public static Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
        {
            Bitmap bmpOut = null;
            try
            {
                Bitmap loBMP = new Bitmap(lcFilename);
                ImageFormat loFormat = loBMP.RawFormat;

                decimal lnRatio;
                int lnNewWidth = 0;
                int lnNewHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                    return loBMP;

                if (loBMP.Width > loBMP.Height)
                {
                    lnRatio = (decimal)lnWidth / loBMP.Width;
                    lnNewWidth = lnWidth;
                    decimal lnTemp = loBMP.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)lnHeight / loBMP.Height;
                    lnNewHeight = lnHeight;
                    decimal lnTemp = loBMP.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }
                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                using (Graphics gr = Graphics.FromImage(bmpOut))
                {
                    gr.SmoothingMode = SmoothingMode.HighQuality;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gr.DrawImage((Image)loBMP, new Rectangle(0, 0, lnNewWidth, lnNewHeight));
                }

                //bmpOut = (Bitmap)loBMP.GetThumbnailImage(lnNewWidth, lnNewHeight, new Image.GetThumbnailImageAbort(ImageHelper.ThumbnailCallback), IntPtr.Zero);
                //Graphics g = Graphics.FromImage(bmpOut);
                //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                //g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);

                loBMP.Dispose();
            }
            catch (Exception ex)
            {
                // LogHelper.WriteError(ex.Message);
            }

            return bmpOut;
        }
        private static void CreateAmpImage(string fileName, string sourceFile, FileUploadReq uploadReq)
        {
            var dirCurrentDay = DateTime.Now.ToString("yyyy/MM/dd");
            var virtualPath = Path.Combine(uploadReq.AmpImageSettings.PrefixPath, dirCurrentDay.Replace("/", "\\"));
            var ampDir = Path.Combine(uploadReq.RootDir, virtualPath);
            if (!Directory.Exists(ampDir))
            {
                Directory.CreateDirectory(ampDir);
            }

            ampDir += "\\";
            if (File.Exists(ampDir + fileName))
            {
                File.Delete(ampDir + fileName);
            }

            Image ampImage;
            Image sourceImage = Image.FromFile(sourceFile);
            ImageFormat imageFormat = new ImageFormat(sourceImage.RawFormat.Guid);

            if (sourceImage.Width > uploadReq.AmpImageSettings.Width)
            {
                ampImage = ImageHelper.Crop(sourceImage, uploadReq.AmpImageSettings.Width, uploadReq.AmpImageSettings.Height, ImageHelper.AnchorPosition.Center);
                ampImage.Save(ampDir + "Original_" + fileName, imageFormat);
            }
            else
            {
                int percent = 100;
                if (sourceImage.Width > 0)
                {
                    percent = 100 * uploadReq.AmpImageSettings.Width / sourceImage.Width;
                }
                ampImage = ImageHelper.ScaleByPercent(sourceImage, percent);
                ampImage = ImageHelper.Crop(ampImage, uploadReq.AmpImageSettings.Width, uploadReq.AmpImageSettings.Height, ImageHelper.AnchorPosition.Center);
                ampImage.Save(ampDir + "Original_" + fileName, imageFormat);
            }
            ampImage.Dispose();
            sourceImage.Dispose();

            // add water mark
            using (Image image = Image.FromFile(ampDir + "Original_" + fileName))
            using (Image watermarkImage = Image.FromFile(Path.Combine(uploadReq.RootDir, uploadReq.AmpImageSettings.ImageWaterMark)))
            {
                Bitmap m_AmpOrg = new Bitmap(image);
                int height = (image.Width + 30) * watermarkImage.Height / watermarkImage.Width;
                Image m_ImageTemp = ImageHelper.FixedSize(watermarkImage, image.Width + 30, height);
                Bitmap m_Wattermask = new Bitmap(m_ImageTemp);

                Bitmap m_Bitmap = ImageHelper.WatermarkImage(m_AmpOrg, m_Wattermask, new Point(-20, m_AmpOrg.Height - m_Wattermask.Height), 25);

                m_Bitmap.Save(ampDir + fileName, imageFormat);
            }

            if (uploadReq.UsingUploadApi)
            {
                var apiResult = ProcessUploadToRemoteServer("Original_" + fileName, ampDir + "Original_" + fileName, virtualPath, uploadReq).GetAwaiter().GetResult();
                if (apiResult.Message != "OK")
                {
                    throw new Exception("Lỗi upload file amp image (remote server)");
                }
                File.Delete(ampDir + "Original_" + fileName);

                apiResult = ProcessUploadToRemoteServer(fileName, ampDir + fileName, virtualPath, uploadReq).GetAwaiter().GetResult();
                if (apiResult.Message != "OK")
                {
                    throw new Exception("Lỗi upload file amp image (remote server)");
                }
                File.Delete(ampDir + fileName);
            }
        }
    }
}
