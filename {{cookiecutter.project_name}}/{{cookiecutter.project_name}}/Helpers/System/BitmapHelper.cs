using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Helpers
{
    public static class BitmapHelper
    {
        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        public static Bitmap Zoom(this Bitmap image, int width)
        {
            if (width >= image.Width)
                return image;

            int thumbHeight = image.Height * width / image.Width;
            Bitmap thumbBitmap = new Bitmap(width, thumbHeight);

            using (Graphics graphics = Graphics.FromImage(thumbBitmap))
            {
                graphics.PixelOffsetMode = PixelOffsetMode.Half;
                graphics.InterpolationMode = InterpolationMode.High;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.Transparent);
                graphics.DrawImage(image, new Rectangle(0, 0, width, thumbHeight), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return thumbBitmap;
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">原图</param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Bitmap Cut(this Bitmap image, Rectangle rectangle)
        {
            Bitmap thumbBitmap = new Bitmap(rectangle.Width, rectangle.Height);

            using (Graphics graphics = Graphics.FromImage(thumbBitmap))
            {
                graphics.PixelOffsetMode = PixelOffsetMode.Half;
                graphics.InterpolationMode = InterpolationMode.High;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.Transparent);
                graphics.DrawImage(image, new Rectangle(0, 0, rectangle.Width, rectangle.Height), rectangle, GraphicsUnit.Pixel);
            }
            return thumbBitmap;
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">原图</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap Cut(this Bitmap image, int x, int y, int width, int height)
        {
            return Cut(image, new Rectangle(x, y, width, height));
        }
    }
}
