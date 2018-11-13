using ImageManipulate;
using System.Drawing;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"../../../../../imgs/";
            Bitmap original_image = (Bitmap)Image.FromFile(path + "test_image.jpg");

            // Обычный медианный фильтр
            Bitmap median_image = Filter.MedianFilter(original_image, 3);
            median_image.Save(path + "MedianFilter_image.jpg");

            // Картинку в серый цвет
            Bitmap gray_image = Filter.ImageToGray(original_image);
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
            Bitmap posttreatment_image = Filter.PostTreatment(doubletres_image);
            posttreatment_image.Save(path + "posttreatment_image.jpg");

            //Алгоритм Хафа для нахождени линий
            Bitmap h_image = Filter.ApplyHoughAlgoritm(doubletres_image, out hough_array);
            Bitmap image_with_lines = Filter.DrawingLineOnImage(original_image, hough_array, 90);
            image_with_lines.Save(path + "base_img_with_lines.jpg");


            //Проверка времени выполнения копирования изображений различными методами
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            System.Collections.Generic.List<long> _new = new System.Collections.Generic.List<long>();
            System.Collections.Generic.List<long> _clone = new System.Collections.Generic.List<long>();
            System.Collections.Generic.List<long> _copy = new System.Collections.Generic.List<long>();
            
            sw.Start();
            long last_time = sw.ElapsedTicks;

            for (int i = 0; i < 1000; i++)
            {
                Bitmap new_bitmap = new Bitmap(original_image);
                _new.Add(sw.ElapsedTicks - last_time);
                last_time = sw.ElapsedTicks;

                Bitmap clone = (Bitmap)original_image.Clone();
                _clone.Add(sw.ElapsedTicks - last_time);
                last_time = sw.ElapsedTicks;

                Bitmap copy = ManipulateImage.CopyImage(original_image);
                _copy.Add(sw.ElapsedTicks - last_time);
                last_time = sw.ElapsedTicks;
            }

            _new.Sort();
            _clone.Sort();
            _copy.Sort();
            
            System.Console.WriteLine("Медианные значения количества тиков:\n" +
            string.Format("Создание нового объекта через new Bitmap(): Max: {0:10} Mid: {1:10} Min: {2:10}\n", _new[999], _new[500], _new[0]) +
            string.Format("Создание нового объекта через Clone():      Max: {0:10} Mid: {1:10} Min: {2:10}\n", _clone[999], _clone[500], _clone[0]) + 
            string.Format("Создание нового объекта через CopyImage():  Max: {0:10} Mid: {1:10} Min: {2:10}\n", _copy[999], _copy[500], _copy[0])
            );
            System.Console.ReadKey();
        }
    }
}
