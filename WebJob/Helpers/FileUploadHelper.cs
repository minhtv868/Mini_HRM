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
using WebJob.Models;

namespace WebJob.Helpers
{
	public class FileUploadHelper
	{
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
				LogHelper.WriteLog(((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name, ex.ToString());
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
				LogHelper.WriteLog(ex.ToString(), ((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name);
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
				LogHelper.WriteLog(((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name, ex.ToString());
			}

			return duration;
		}
	}
}
