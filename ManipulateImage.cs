using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ImageManipulate
{
    public static class ManipulateImage
    {
        public static Bitmap Resize(Image img, int w, int h)
        {
            Bitmap bmp = new Bitmap(img, w, h);
            return bmp;
        }

        public static Image Rotate(Image img, float angle)
        {
            int h = img.Height, w = img.Width;
            Bitmap bmp = new Bitmap(w, h);

            Graphics gfx = Graphics.FromImage(bmp);

            gfx.TranslateTransform((float)w / 2, (float)h / 2);

            gfx.RotateTransform(angle);

            gfx.TranslateTransform(-(float)w / 2, -(float)h / 2);

            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            gfx.DrawImage(img, new Point(w / 2 - img.Width / 2, h / 2 - img.Height / 2));

            gfx.Dispose();

            return bmp;
        }

        public static void Combine(string backgroundPath, string newLayerPath, string saveToPath, int x, int y)
        {
            Image background_img = Image.FromFile(backgroundPath);
            Bitmap resultImage = new Bitmap(background_img.Width, background_img.Height);

            using (Graphics graphics = Graphics.FromImage(resultImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(background_img, 0, 0, resultImage.Width, resultImage.Height);
                Image img = Image.FromFile(newLayerPath);
                graphics.DrawImage(img, x, y, img.Width, img.Height);
            }

            resultImage.Save(saveToPath);
        }

        public static Bitmap Combine(Image background, Image number, int x, int y)
        {
            Image background_img = background;
            Bitmap resultImage = new Bitmap(background_img.Width, background_img.Height);

            using (Graphics graphics = Graphics.FromImage(resultImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(background_img, 0, 0, resultImage.Width, resultImage.Height);

                Image img = number;
                graphics.DrawImage(img, x, y, img.Width, img.Height);

            }

            return resultImage;
        }

    }

}
