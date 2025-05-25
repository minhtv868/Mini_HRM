using IC.Application.Common.Constants;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace IC.WebJob.Helpers
{
	public class ImageHelper
    {
		public enum Dimensions
		{
			Width,
			Height
		}

		public enum AnchorPosition
		{
			Top,
			Center,
			Bottom,
			Left,
			Right
		}
		/// <summary>
		/// Không dùng nữa, nếu cần dùng thì sửa để phù hợp .net 8 và xóa comment này
		/// </summary>
		/// <param name="imgPhoto"></param>
		/// <param name="Percent"></param>
		/// <returns></returns>
		public static Image ScaleByPercent(Image imgPhoto, int Percent)
		{
            if (!OperatingSystem.IsWindows()) return null;

			float nPercent = ((float)Percent / 100);

			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;

			int destX = 0;
			int destY = 0;
			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
				new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}
        /// <summary>
        /// Không dùng nữa, nếu cần dùng thì sửa để phù hợp .net 8 và xóa comment này
        /// </summary>
        /// <param name="imgPhoto"></param>
        /// <param name="Size"></param>
        /// <param name="Dimension"></param>
        /// <returns></returns>
        public static Image ConstrainProportions(Image imgPhoto, int Size, Dimensions Dimension)
		{
            if (!OperatingSystem.IsWindows()) return null;

			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0;
            float nPercent;
            switch (Dimension)
			{
				case Dimensions.Width:
					nPercent = ((float)Size / (float)sourceWidth);
					break;
				default:
					nPercent = ((float)Size / (float)sourceHeight);
					break;
			}

			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
			new Rectangle(destX, destY, destWidth, destHeight),
			new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
			GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}
        /// <summary>
        /// Không dùng nữa, nếu cần dùng thì sửa để phù hợp .net 8 và xóa comment này
        /// </summary>
        /// <param name="imgPhoto"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static Image FixedSize(Image imgPhoto, int Width, int Height)
		{
            if (!OperatingSystem.IsWindows()) return null;

			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0;
            float nPercentW = Width / (float)sourceWidth;
            float nPercentH = Height / (float)sourceHeight;
            float nPercent;
            //if we have to pad the height pad both the top and the bottom
            //with the difference between the scaled height and the desired height
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = (int)((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = (int)((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.Clear(Color.White);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
				new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}
        /// <summary>
        /// Không dùng nữa, nếu cần dùng thì sửa để phù hợp .net 8 và xóa comment này
        /// </summary>
        /// <param name="imgPhoto"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Anchor"></param>
        /// <returns></returns>
        public static Image Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
		{
            if (!OperatingSystem.IsWindows()) return null;

			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0;
            float nPercentW = Width / (float)sourceWidth;
            float nPercentH = Height / (float)sourceHeight;
            float nPercent;
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                switch (Anchor)
                {
                    case AnchorPosition.Top:
                        destY = 0;
                        break;
                    case AnchorPosition.Bottom:
                        destY = (int)(Height - (sourceHeight * nPercent));
                        break;
                    default:
                        destY = (int)((Height - (sourceHeight * nPercent)) / 2);
                        break;
                }
            }
            else
            {
                nPercent = nPercentH;
                switch (Anchor)
                {
                    case AnchorPosition.Left:
                        destX = 0;
                        break;
                    case AnchorPosition.Right:
                        destX = (int)(Width - (sourceWidth * nPercent));
                        break;
                    default:
                        destX = (int)((Width - (sourceWidth * nPercent)) / 2);
                        break;
                }
            }

            int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
				new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX + 5, sourceY + 5, sourceWidth - 5, sourceHeight - 5),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}
        /// <summary>
        /// Không dùng nữa, nếu cần dùng thì sửa để phù hợp .net 8 và xóa comment này
        /// </summary>
        /// <param name="imgPhoto"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static Image ResizeV2(Image imgPhoto, int Width, int Height)
		{
            if (!OperatingSystem.IsWindows()) return null;

			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
            float nPercentW = Width / (float)sourceWidth;
            float nPercentH = Height / (float)sourceHeight;
            float nPercent;
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap b = new Bitmap(destWidth, destHeight);
			Graphics g = Graphics.FromImage(b);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			g.DrawImage(imgPhoto, 0, 0, destWidth, destHeight);
			g.Dispose();

			return b;
		}
        /// <summary>
        /// Không dùng nữa, nếu cần dùng thì sửa để phù hợp .net 8 và xóa comment này
        /// </summary>
        /// <param name="image"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static Image Fit2PictureBox(Image image, int Width, int Height)
		{
            if (!OperatingSystem.IsWindows()) return null;

			Graphics g;

			// Scale:
			double scaleY = (double)image.Width / Width;
			double scaleX = (double)image.Height / Height;
			double scale = scaleY < scaleX ? scaleX : scaleY;

            // Create new bitmap:
            Bitmap bmp = new Bitmap(
                (int)(image.Width / scale),
                (int)(image.Height / scale));

            // Set resolution of the new image:
            bmp.SetResolution(
				image.HorizontalResolution,
				image.VerticalResolution);

			// Create graphics:
			g = Graphics.FromImage(bmp);

			// Set interpolation mode:
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.Clear(Color.Transparent);
			// Draw the new image:
			g.DrawImage(
				image,
				new Rectangle(            // Destination
					0, 0,
					bmp.Width, bmp.Height),
				//image.Width, image.Height),
				new Rectangle(            // Source
					5, 5,
					image.Width - 5, image.Height - 5),
				GraphicsUnit.Pixel);

			// Release the resources of the graphics:
			g.Dispose();

			// Release the resources of the origin image:
			//image.Dispose();

			return bmp;
		}
        /// <summary>
        /// Không dùng nữa, nếu cần dùng thì sửa để phù hợp .net 8 và xóa comment này
        /// </summary>
        /// <param name="ImageToWatermark"></param>
        /// <param name="Watermark"></param>
        /// <param name="WatermarkPosition"></param>
        /// <param name="Opacity"></param>
        /// <returns></returns>
        public static Bitmap WatermarkImage(Bitmap ImageToWatermark, Bitmap Watermark, Point WatermarkPosition, float Opacity)
		{
            if (!OperatingSystem.IsWindows()) return null;

			using (Graphics G = Graphics.FromImage(ImageToWatermark))
			{
				using (ImageAttributes IA = new ImageAttributes())
				{
                    ColorMatrix CM = new ColorMatrix();
					CM.Matrix33 = Opacity;
					IA.SetColorMatrix(CM);
					if (Opacity > 0)
						G.DrawImage(Watermark, new Rectangle(WatermarkPosition, Watermark.Size), 0, 0, Watermark.Width, Watermark.Height, GraphicsUnit.Pixel, IA);
					else
						G.DrawImage(Watermark, new Rectangle(WatermarkPosition, Watermark.Size));
				}
			}
			return ImageToWatermark;
		}

		public static Image Thumbnail(Image image, int Width, int Height)
		{
            if (!OperatingSystem.IsWindows()) return null;

			Image thumbnailImage = image.GetThumbnailImage(Width, Height, new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
			return thumbnailImage;
		}

		public static bool ThumbnailCallback()
		{
			return true;
        }
        public static bool IsThumbLinkImage(string image)
        {
			return (!string.IsNullOrEmpty(image) && (image.StartsWith("/uploads/")));
        } 

        public static string GetThumbLinkImage(string image)
		{
			if (IsThumbLinkImage(image))
			{
				if (image.StartsWith("/uploads/"))
				{ 
                    image = AppConstant.MediaDomain + image; 
				}
            }

			return image;
		}
	}
}
