using ImageManipulate;
using System.Drawing;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"../../../../../imgs/";
            Bitmap base_image = (Bitmap)Image.FromFile(path + "test_image.jpg");

            // Обычный медианный фильтр
            Bitmap median_image = Filter.MedianFilter(base_image, 3);
            median_image.Save(path + "MedianFilter_image.jpg");

            // Картинку в серый цвет
            Bitmap gray_image = Filter.ImageToGray(base_image);
            gray_image.Save(path + "gray_image.jpg");

            // Размытие матрицами
            double koef = 1d / 16d;
            Bitmap gaus_image = Filter.ApplySmoothMethod(gray_image, Matrix.Gauss3x3, koef);
            gaus_image.Save(path + "gausBlur_image.jpg");
            
            // Доп. Массивы для нахождений линий на изображении
            int[,] hough_array;
            double[,] atans;

            //Применения оператора Собеля
            Bitmap sobel_image = Filter.ApplySobelMethod(gaus_image, Matrix.SobelVertical, Matrix.SobelGorizontal, out atans);
            Bitmap nonmax_image = Filter.NonMaximum(sobel_image, atans);
            nonmax_image.Save(path + "nonmax_image.jpg");

            //постобработка изображения
            Bitmap doubletres_image = Filter.ApplyDoubleTresholding(nonmax_image, 0.3d, 0.7d);
            Filter.PostTreatment(doubletres_image);
            doubletres_image.Save(path + "doubletres_image.jpg");

            //Алгоритм Хафа для нахождени линий
            Bitmap h_image = Filter.ApplyHoughAlgoritm(doubletres_image, out hough_array);
            Filter.DrawingLineOnImage(base_image, hough_array, 90);
            base_image.Save("base_img_with_lines.jpg");

        }
    }
}
