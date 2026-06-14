using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace ResumeBuilder.Helpers
{
    public static class ImageHelper
    {
        /// <summary>
        /// Resize an image to the specified dimensions.
        /// </summary>
        public static Image ResizeImage(string imagePath, int width, int height)
        {
            using var original = Image.FromFile(imagePath);
            var resized = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(resized);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.DrawImage(original, 0, 0, width, height);
            return resized;
        }

        /// <summary>
        /// Convert an image file to a Base64 string.
        /// </summary>
        public static string ConvertToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return System.Convert.ToBase64String(imageBytes);
        }

        /// <summary>
        /// Check if a file is a valid image.
        /// </summary>
        public static bool IsValidImage(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();
            return ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp";
        }

        /// <summary>
        /// Crops a source image into a perfect circle using GDI+ and outputs a temporary file path.
        /// </summary>
        public static string CropImageToCircle(string imagePath)
        {
            try
            {
                using (var srcImage = Image.FromFile(imagePath))
                {
                    int minSize = System.Math.Min(srcImage.Width, srcImage.Height);
                    var dstImage = new Bitmap(minSize, minSize);
                    using (var g = Graphics.FromImage(dstImage))
                    {
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        
                        using (var brush = new TextureBrush(srcImage))
                        {
                            // Center texture
                            brush.TranslateTransform((minSize - srcImage.Width) / 2f, (minSize - srcImage.Height) / 2f);
                            using (var path = new GraphicsPath())
                            {
                                path.AddEllipse(0, 0, minSize, minSize);
                                g.FillPath(brush, path);
                            }
                        }
                    }
                    string tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(imagePath) + "_circle.png");
                    dstImage.Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
                    return tempPath;
                }
            }
            catch
            {
                return imagePath; // Fallback to original image if failed
            }
        }
    }
}
