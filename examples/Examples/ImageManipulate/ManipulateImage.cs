using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ImageManipulate
{
    /// <summary>
    /// Класс методов для манипуляций с изображениями, посредством доступных инструментов из System.Drawing.
    /// (Кроме методов CutToXAndY и CopyImage)
    /// </summary>
    public static class ManipulateImage
    {
        /// <summary>
        /// Метод изменяет размер изображения.
        /// </summary>
        /// <param name="img">Исходное изображение</param>
        /// <param name="w">Необходимая ширина.</param>
        /// <param name="h">Необходимая высота.</param>
        /// <returns>Изображение необхоимого размера.</returns>
        public static Bitmap Resize(Image img, int w, int h)
        {
            Bitmap bmp = new Bitmap(img, w, h);
            return bmp;
        }
        /// <summary>
        /// Поворачивает изображение вокруг центра на заданный угол.
        /// </summary>
        /// <param name="img">Исходное изображение.</param>
        /// <param name="angle">Угол в градусах.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод накладывает одно изображение на другое по заданным координатам.
        /// </summary>
        /// <param name="backgroundPath">Путь к фоновму изображению.</param>
        /// <param name="newLayerPath">Путь к накладываемому изображению.</param>
        /// <param name="saveToPath">Путь для сохранения.</param>
        /// <param name="x">Координата X на фоновом изображении.</param>
        /// <param name="y">Координата Y на фоновом изображении.</param>
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
        /// <summary>
        /// Метод накладывает одно изображение на другое по заданным координатам.
        /// </summary>
        /// <param name="background">Фоновое изображение.</param>
        /// <param name="overlayImg">Накладываемое изображение.</param>
        /// <param name="x">Координата X на фоновом изображении.</param>
        /// <param name="y">Координата X на фоновом изображении.</param>
        /// <returns></returns>
        public static Bitmap Combine(Image background, Image overlayImg, int x, int y)
        {
            Image background_img = background;
            Bitmap resultImage = new Bitmap(background_img.Width, background_img.Height);

            using (Graphics graphics = Graphics.FromImage(resultImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(background_img, 0, 0, resultImage.Width, resultImage.Height);

                Image img = overlayImg;
                graphics.DrawImage(img, x, y, img.Width, img.Height);

            }

            return resultImage;
        }
        /// <summary>
        /// Метод обрезает изображение по заданным координатам.
        /// </summary>
        /// <param name="source">Исходное изображение.</param>
        /// <param name="leftX">Координата X левого верхнего угла.</param>
        /// <param name="rightX">Координата X правого нижнего угла.</param>
        /// <param name="topY">Координата Y левого верхнего угла.</param>
        /// <param name="botY">Координата Y правого нижнего угла</param>
        /// <returns>Изображение между координатами.</returns>
        public static Bitmap CutToXAndY(Bitmap source, int leftX, int rightX, int topY, int botY)
        {
            var width = rightX - leftX;
            var height = botY - topY;

            var sourceData = source.LockBits(new Rectangle(new System.Drawing.Point(0, 0), source.Size),
                ImageLockMode.ReadOnly,
                source.PixelFormat);

            var result = new Bitmap(width, height, source.PixelFormat);
            var resultData = result.LockBits(new Rectangle(new System.Drawing.Point(0, 0), result.Size),
                ImageLockMode.ReadWrite,
                source.PixelFormat);


            var sourceStride = sourceData.Stride;
            var resultStride = resultData.Stride;

            var sourceScan0 = sourceData.Scan0;
            var resultScan0 = resultData.Scan0;

            var resultPixelSize = resultStride / width;
            unsafe
            {
                for (var y = topY; y < topY + height; y++)
                {
                    var sourceRow = (byte*)sourceScan0 + (y * sourceStride);
                    var resultRow = (byte*)resultScan0 + ((y - topY) * resultStride);

                    for (var x = leftX; x < leftX + width; x++)
                    {
                        resultRow[(x - leftX) * resultPixelSize] = sourceRow[x * resultPixelSize];
                        resultRow[(x - leftX) * resultPixelSize + 1] = sourceRow[x * resultPixelSize + 1];
                        resultRow[(x - leftX) * resultPixelSize + 2] = sourceRow[x * resultPixelSize + 2];

                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);
            return result;
        }
        /// <summary>
        /// Метод копирует изображение.
        /// </summary>
        /// <param name="image">Исходное изображение.</param>
        /// <returns>Новый экземпляр изображения.</returns>
        public static Bitmap CopyImage(Bitmap image)
        {
            var width = image.Width;
            var height = image.Height;

            var sourceData = image.LockBits(new Rectangle(new System.Drawing.Point(0, 0), image.Size),
                ImageLockMode.ReadOnly,
                image.PixelFormat);

            var result = new Bitmap(width, height, image.PixelFormat);
            var resultData = result.LockBits(new Rectangle(new System.Drawing.Point(0, 0), result.Size),
                ImageLockMode.ReadWrite,
                image.PixelFormat);


            var sourceStride = sourceData.Stride;
            var resultStride = resultData.Stride;

            var sourceScan0 = sourceData.Scan0;
            var resultScan0 = resultData.Scan0;

            var resultPixelSize = resultStride / width;
            unsafe
            {
                for (var y = 0; y < height - 0; y++)
                {
                    var sourceRow = (byte*)sourceScan0 + (y * sourceStride);
                    var resultRow = (byte*)resultScan0 + (y * resultStride);

                    for (var x = 0; x < width - 0; x++)
                    {

                        resultRow[x * resultPixelSize] = sourceRow[x * resultPixelSize];
                        resultRow[x * resultPixelSize + 1] = sourceRow[x * resultPixelSize + 1];
                        resultRow[x * resultPixelSize + 2] = sourceRow[x * resultPixelSize + 2];

                    }
                }
            }

            image.UnlockBits(sourceData);
            result.UnlockBits(resultData);
            return result;
        }
    }

}
