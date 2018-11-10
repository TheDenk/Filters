using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ImageManipulate
{
    public static class Filter
    {
        public static Bitmap ApplySobelMethod(Bitmap source, double[,] verticaltMatrix, double[,] gorizontalMatrix, out double[,] atanArray, double koef = 1)
        {
            var width = source.Width;
            var height = source.Height;

            int border = (int)verticaltMatrix.GetLength(0) / 2;

            var sourceData = source.LockBits(new Rectangle(new System.Drawing.Point(0, 0), source.Size),
                ImageLockMode.ReadOnly,
                source.PixelFormat);

            var result = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            var resultData = result.LockBits(new Rectangle(new System.Drawing.Point(0, 0), result.Size),
                ImageLockMode.ReadWrite,
                source.PixelFormat);


            var sourceStride = sourceData.Stride;
            var resultStride = resultData.Stride;

            var sourceScan0 = sourceData.Scan0;
            var resultScan0 = resultData.Scan0;

            var resultPixelSize = resultStride / width;
            atanArray = new double[height, width];

            unsafe
            {
                for (var y = border; y < height - border; y++)
                {

                    var resultRow = (byte*)resultScan0 + (y * resultStride);



                    for (var x = border; x < width - border; x++)
                    {
                        double blueX = 0, blueY = 0,
                            greenX = 0, greenY = 0,
                            redX = 0, redY = 0;

                        for (int i = -border; i <= border; i++)
                        {
                            var sourceRow = (byte*)sourceScan0 + ((y + i) * sourceStride);

                            for (int j = -border; j <= border; j++)
                            {

                                blueX += sourceRow[(x + j) * resultPixelSize] * gorizontalMatrix[i + border, j + border];
                                greenX += sourceRow[(x + j) * resultPixelSize + 1] * gorizontalMatrix[i + border, j + border];
                                redX += sourceRow[(x + j) * resultPixelSize + 2] * gorizontalMatrix[i + border, j + border];

                                blueY += sourceRow[(x + j) * resultPixelSize] * verticaltMatrix[i + border, j + border];
                                greenY += sourceRow[(x + j) * resultPixelSize + 1] * verticaltMatrix[i + border, j + border];
                                redY += sourceRow[(x + j) * resultPixelSize + 2] * verticaltMatrix[i + border, j + border];
                            }
                        }

                        atanArray[y, x] = Math.Atan(redY /
                                                   redX) * 180 / Math.PI;

                        var red = koef * Math.Sqrt(redX * redX + redY * redY);
                        var green = koef * Math.Sqrt(greenX * greenX + greenY * greenY);
                        var blue = koef * Math.Sqrt(blueX * blueX + blueY * blueY);

                        if (red > 255) red = 255;
                        if (red < 1) red = 0;
                        if (green > 255) green = 255;
                        if (green < 1) green = 0;
                        if (blue > 255) blue = 255;
                        if (blue < 1) blue = 0;

                        resultRow[x * resultPixelSize] = (byte)blue;
                        resultRow[x * resultPixelSize + 1] = (byte)green;
                        resultRow[x * resultPixelSize + 2] = (byte)red;

                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);
            return result;
        }
        public static Bitmap ApplySmoothMethod(Bitmap source, double[,] matrix, double koef)
        {
            var width = source.Width;
            var height = source.Height;
            int border = (int)matrix.GetLength(0) / 2;

            var sourceData = source.LockBits(new Rectangle(new System.Drawing.Point(0, 0), source.Size),
                ImageLockMode.ReadOnly,
                source.PixelFormat);

            var result = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            var resultData = result.LockBits(new Rectangle(new System.Drawing.Point(0, 0), result.Size),
                ImageLockMode.ReadWrite,
                source.PixelFormat);


            var sourceStride = sourceData.Stride;
            var resultStride = resultData.Stride;

            var sourceScan0 = sourceData.Scan0;
            var resultScan0 = resultData.Scan0;

            var resultPixelSize = resultStride / width;

            double blue = 0,
                green = 0,
                red = 0;

            unsafe
            {
                for (var y = 0; y < height; y++)
                {

                    var resultRow = (byte*)resultScan0 + (y * resultStride);
                    var sorseRow = (byte*)sourceScan0 + (y * resultStride);
                    for (var x = 0; x < width - 0; x++)
                    {
                        if (x < border || y < border || y == height - border || x == width - border)
                        {
                            resultRow[x * resultPixelSize] = sorseRow[x * resultPixelSize];
                            resultRow[x * resultPixelSize + 1] = sorseRow[x * resultPixelSize + 1];
                            resultRow[x * resultPixelSize + 2] = sorseRow[x * resultPixelSize + 2];
                            continue;
                        }
                        for (int i = -border; i <= border; i++)
                        {
                            var sourceRow = (byte*)sourceScan0 + ((y + i) * sourceStride);
                            for (int j = -border; j <= border; j++)
                            {
                                blue += sourceRow[(x + j) * resultPixelSize] * matrix[i + border, j + border];
                                green += sourceRow[(x + j) * resultPixelSize + 1] * matrix[i + border, j + border];
                                red += sourceRow[(x + j) * resultPixelSize + 2] * matrix[i + border, j + border];

                            }
                        }
                        blue = blue * koef;
                        green = green * koef;
                        red = red * koef;
                        if (green > 255) green = 255;
                        if (green < 1) green = 0;
                        if (blue > 255) blue = 255;
                        if (blue < 1) blue = 0;
                        if (red > 255) red = 255;
                        if (red < 1) red = 0;

                        resultRow[x * resultPixelSize] = (byte)blue;
                        resultRow[x * resultPixelSize + 1] = (byte)green;
                        resultRow[x * resultPixelSize + 2] = (byte)red;

                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);
            return result;
        }
        public static Bitmap CreateBinaryImgFromRgb(Bitmap source, int treshold = 115)
        {
            var width = source.Width;
            var height = source.Height;

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
                for (var y = 0; y < height - 0; y++)
                {
                    var sourceRow = (byte*)sourceScan0 + (y * sourceStride);
                    var resultRow = (byte*)resultScan0 + (y * resultStride);

                    for (var x = 0; x < width - 0; x++)
                    {

                        var v = (byte)(0.3 * sourceRow[x * resultPixelSize + 2] + 1 * sourceRow[x * resultPixelSize + 1] + 0.11 * sourceRow[x * resultPixelSize] < treshold
                            ? 0 : 255);
                        resultRow[x * resultPixelSize] = v;
                        resultRow[x * resultPixelSize + 1] = v;
                        resultRow[x * resultPixelSize + 2] = v;

                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);
            return result;
        }
        public static Bitmap MedianFilter(Bitmap sourceBitmap, int matrixSize)
        {
            var sourceData =
                sourceBitmap.LockBits(new Rectangle(0, 0,
                        sourceBitmap.Width, sourceBitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride *
                                          sourceData.Height];


            byte[] resultBuffer = new byte[sourceData.Stride *
                                           sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            int filterOffset = (matrixSize - 1) / 2;
            int calcOffset = 0;


            int byteOffset = 0;


            List<int> neighbourPixels = new List<int>();
            byte[] middlePixel;


            for (int offsetY = filterOffset; offsetY <
                                             sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                                                 sourceBitmap.Width - filterOffset; offsetX++)
                {
                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;


                    neighbourPixels.Clear();


                    for (int filterY = -filterOffset;
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {


                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);


                            neighbourPixels.Add(BitConverter.ToInt32(
                                pixelBuffer, calcOffset));
                        }
                    }


                    neighbourPixels.Sort();

                    middlePixel = BitConverter.GetBytes(
                        neighbourPixels[filterOffset]);


                    resultBuffer[byteOffset] = middlePixel[0];
                    resultBuffer[byteOffset + 1] = middlePixel[1];
                    resultBuffer[byteOffset + 2] = middlePixel[2];
                    resultBuffer[byteOffset + 3] = middlePixel[3];
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                sourceBitmap.Height);


            BitmapData resultData =
                resultBitmap.LockBits(new Rectangle(0, 0,
                        resultBitmap.Width, resultBitmap.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                resultBuffer.Length);


            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }
        public static Bitmap NonMaximum(Bitmap source, double[,] atanArray)
        {
            var width = source.Width;
            var height = source.Height;

            var sourceData = source.LockBits(new Rectangle(new System.Drawing.Point(0, 0), source.Size),
                ImageLockMode.ReadOnly,
                source.PixelFormat);

            var result = new Bitmap(width, height, PixelFormat.Format32bppRgb);
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
                for (var y = 1; y < height - 1; y++)
                {
                    var sourcePreRow = (byte*)sourceScan0 + ((y - 1) * sourceStride);
                    var sourceRow = (byte*)sourceScan0 + (y * sourceStride);
                    var sourceNextRow = (byte*)sourceScan0 + ((y + 1) * sourceStride);

                    var resultRow = (byte*)resultScan0 + (y * resultStride);

                    for (var x = 1; x < width - 1; x++)
                    {


                        //Horizontal angle, vertical edge (-22.5~22.5)
                        if (atanArray[y, x] < 22.5)
                        {
                            if (sourceRow[x * resultPixelSize] >= sourceRow[(x - 1) * resultPixelSize] &&
                                sourceRow[x * resultPixelSize] >= sourceRow[(x + 1) * resultPixelSize])
                            {
                                resultRow[x * resultPixelSize] = sourceRow[x * resultPixelSize];
                                resultRow[x * resultPixelSize + 1] = sourceRow[x * resultPixelSize + 1];
                                resultRow[x * resultPixelSize + 2] = sourceRow[x * resultPixelSize + 2];
                            }

                        }

                        //Vertical angle, horizontal edge (67.5~90 or -67.5~-90)
                        else if (atanArray[y, x] > 67.5)
                        {
                            if (sourceRow[x * resultPixelSize] >= sourcePreRow[(x) * resultPixelSize] &&
                                 sourceRow[x * resultPixelSize] >= sourceNextRow[(x) * resultPixelSize])
                            {
                                resultRow[x * resultPixelSize] = sourceRow[x * resultPixelSize];
                                resultRow[x * resultPixelSize + 1] = sourceRow[x * resultPixelSize + 1];
                                resultRow[x * resultPixelSize + 2] = sourceRow[x * resultPixelSize + 2];
                            }

                        }

                        //-45 degree angle, +45 Degree Edge
                        else if ((-67.5 < atanArray[y, x]) && (atanArray[y, x] <= -22.5))
                        {
                            if (sourceRow[x * resultPixelSize] >= sourcePreRow[(x - 1) * resultPixelSize] &&
                                sourceRow[x * resultPixelSize] >= sourceNextRow[(x + 1) * resultPixelSize])
                            {
                                resultRow[x * resultPixelSize] = sourceRow[x * resultPixelSize];
                                resultRow[x * resultPixelSize + 1] = sourceRow[x * resultPixelSize + 1];
                                resultRow[x * resultPixelSize + 2] = sourceRow[x * resultPixelSize + 2];
                            }

                        }

                        //45 degree angle, -45 degree Edge
                        else if ((22.5 < atanArray[y, x]) && (atanArray[y, x] <= 67.5))
                        {
                            if (sourceRow[x * resultPixelSize] >= sourceNextRow[(x - 1) * resultPixelSize] &&
                                sourceRow[x * resultPixelSize] >= sourcePreRow[(x + 1) * resultPixelSize])
                            {
                                resultRow[x * resultPixelSize] = sourceRow[x * resultPixelSize];
                                resultRow[x * resultPixelSize + 1] = sourceRow[x * resultPixelSize + 1];
                                resultRow[x * resultPixelSize + 2] = sourceRow[x * resultPixelSize + 2];
                            }

                        }


                    }
                }

            }
            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);
            return result;
        }
        public static Bitmap ImageToGray(Bitmap source)
        {

            var width = source.Width;
            var height = source.Height;

            var sourceData = source.LockBits(new Rectangle(new System.Drawing.Point(0, 0), source.Size),
                ImageLockMode.ReadOnly,
                source.PixelFormat);

            var result = new Bitmap(width, height, PixelFormat.Format32bppRgb);
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
                for (var y = 0; y < height - 0; y++)
                {
                    var sourceRow = (byte*)sourceScan0 + (y * sourceStride);
                    var resultRow = (byte*)resultScan0 + (y * resultStride);

                    for (var x = 0; x < width - 0; x++)
                    {

                        var v = (byte)(0.299 * sourceRow[x * resultPixelSize + 2] + 0.587 * sourceRow[x * resultPixelSize + 1] + 0.114 * sourceRow[x * resultPixelSize]);
                        resultRow[x * resultPixelSize] = v;
                        resultRow[x * resultPixelSize + 1] = v;
                        resultRow[x * resultPixelSize + 2] = v;

                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);
            return result;
        }
        public static Bitmap ApplyDoubleTresholding(Bitmap source, double lowLevel, double highLevel)
        {
            var width = source.Width;
            var height = source.Height;

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
                for (var y = 0; y < height - 0; y++)
                {
                    var sourceRow = (byte*)sourceScan0 + (y * sourceStride);
                    var resultRow = (byte*)resultScan0 + (y * resultStride);

                    for (var x = 0; x < width - 0; x++)
                    {
                        var v = (byte)(sourceRow[x * resultPixelSize] > highLevel * 255 ? 255 :
                            sourceRow[x * resultPixelSize] < lowLevel * 255 ? 0 : 127);
                        resultRow[x * resultPixelSize] = v;
                        resultRow[x * resultPixelSize + 1] = v;
                        resultRow[x * resultPixelSize + 2] = v;

                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);
            return result;
        }
        public static Bitmap ApplyHoughAlgoritm(Bitmap source, out int[,] halfArray, int incrementOfOutArray = 1)
        {
            var width = source.Width;
            var height = source.Height;

            var sourceData = source.LockBits(new Rectangle(new System.Drawing.Point(0, 0), source.Size),
                ImageLockMode.ReadWrite,
                source.PixelFormat);
            int W = (int)(Math.Sqrt(height * height + width * width));
            halfArray = new int[(int)(W * 1.414213562) * 2 + 1, 181];

            var result = new Bitmap(halfArray.GetLength(1), halfArray.GetLength(0), PixelFormat.Format32bppRgb);
            var resultData = result.LockBits(new Rectangle(new System.Drawing.Point(0, 0), result.Size),
                ImageLockMode.ReadWrite,
                source.PixelFormat);


            var sourceStride = sourceData.Stride;
            var resultStride = resultData.Stride;

            var sourceScan0 = sourceData.Scan0;
            var resultScan0 = resultData.Scan0;

            var sourcePixelSize = sourceStride / width;
            var resultPixelSize = resultStride / halfArray.GetLength(1);

            double rad = Math.PI / 180;
            double alfa = rad * -90;
            double[,] sinCos = new double[2, 181];
            for (int i = 0; i < 181; i++)
            {
                sinCos[0, i] = Math.Sin(alfa);
                sinCos[1, i] = Math.Cos(alfa);
                alfa += rad;
            }
            unsafe
            {
                for (var y = 0; y < height; y++)
                {

                    var sourceRow = (byte*)sourceScan0 + (y * sourceStride);


                    for (var x = 0; x < width; x++)
                    {
                        if (sourceRow[x * sourcePixelSize] >= 127)
                        {

                            for (int i = 0; i < 181; i++)
                            {
                                double h = halfArray.GetLength(0) / 2;
                                int p = (int)((x * sinCos[1, i] + y * sinCos[0, i]) + h);
                                halfArray[p, i] += incrementOfOutArray;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < halfArray.GetLength(0); i++)
            {
                for (int j = 0; j < halfArray.GetLength(1); j++)
                {
                    halfArray[i, j] = halfArray[i, j] > 255 ? 255 : halfArray[i, j];
                }
            }
            unsafe
            {
                for (var y = 0; y < halfArray.GetLength(0); y++)
                {

                    var resultRow = (byte*)resultScan0 + (y * resultStride);

                    for (var x = 0; x < halfArray.GetLength(1); x++)
                    {
                        resultRow[x * resultPixelSize] = (byte)(halfArray[y, x]);
                        resultRow[x * resultPixelSize + 1] = (byte)(halfArray[y, x]);
                        resultRow[x * resultPixelSize + 2] = (byte)(halfArray[y, x]);
                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);

            return result;
        }
        public static Bitmap ModifiedApplyHafAlgoritm(Bitmap source, out int[,] halfArray, int incrementOfOutArray = 1)
        {
            var width = source.Width;
            var height = source.Height;

            var sourceData = source.LockBits(new Rectangle(new System.Drawing.Point(0, 0), source.Size),
                ImageLockMode.ReadWrite,
                source.PixelFormat);
            int W = (int)(Math.Sqrt(height * height + width * width));
            halfArray = new int[(int)(W * 1.414213562) * 2 + 1, 181];

            var result = new Bitmap(halfArray.GetLength(1), halfArray.GetLength(0), PixelFormat.Format32bppRgb);
            var resultData = result.LockBits(new Rectangle(new System.Drawing.Point(0, 0), result.Size),
                ImageLockMode.ReadWrite,
                source.PixelFormat);


            var sourceStride = sourceData.Stride;
            var resultStride = resultData.Stride;

            var sourceScan0 = sourceData.Scan0;
            var resultScan0 = resultData.Scan0;

            var sourcePixelSize = sourceStride / width;
            var resultPixelSize = resultStride / halfArray.GetLength(1);

            double rad = Math.PI / 180;
            double alfa = rad * -90;
            double[,] sinCos = new double[2, 181];
            for (int i = 0; i < 181; i++)
            {
                sinCos[0, i] = Math.Sin(alfa);
                sinCos[1, i] = Math.Cos(alfa);
                alfa += rad;
            }
            unsafe
            {
                for (var y = 0; y < height; y++)
                {

                    var sourceRow = (byte*)sourceScan0 + (y * sourceStride);


                    for (var x = 0; x < width; x++)
                    {
                        if (sourceRow[x * sourcePixelSize] >= 127)
                        {

                            for (int i = 0; i < 181; i++)
                            {
                                if ((((i - 90) > -90) && ((i - 90) < -80)) || (((i - 90) > 80) && ((i - 90) < 90)) || (((i - 90) > -10) && ((i - 90) < 10)))
                                {
                                    double h = halfArray.GetLength(0) / 2;
                                    int p = (int)((x * sinCos[1, i] + y * sinCos[0, i]) + h);
                                    halfArray[p, i] += incrementOfOutArray;
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < halfArray.GetLength(0); i++)
            {
                for (int j = 0; j < halfArray.GetLength(1); j++)
                {
                    halfArray[i, j] = halfArray[i, j] > 255 ? 255 : halfArray[i, j];
                }
            }
            unsafe
            {
                for (var y = 0; y < halfArray.GetLength(0); y++)
                {

                    var resultRow = (byte*)resultScan0 + (y * resultStride);

                    for (var x = 0; x < halfArray.GetLength(1); x++)
                    {
                        resultRow[x * resultPixelSize] = (byte)(halfArray[y, x]);
                        resultRow[x * resultPixelSize + 1] = (byte)(halfArray[y, x]);
                        resultRow[x * resultPixelSize + 2] = (byte)(halfArray[y, x]);
                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);

            return result;
        }
        public static void DrawingLineOnImage(Bitmap source, int[,] halfArray, double level)
        {
            var width = source.Width;
            var height = source.Height;

            var sourceData = source.LockBits(new Rectangle(new System.Drawing.Point(0, 0), source.Size),
                ImageLockMode.ReadWrite,
                source.PixelFormat);



            var sourceStride = sourceData.Stride;

            var sourceScan0 = sourceData.Scan0;

            var resultPixelSize = sourceStride / width;

            double rad = Math.PI / 180;

            double alfa = rad * -90;
            double[,] sinCos = new double[2, 181];
            for (int i = 0; i < 181; i++)
            {

                sinCos[0, i] = Math.Sin(alfa);
                sinCos[1, i] = Math.Cos(alfa);
                alfa += rad;

            }

            var Normals = new List<int>();
            var Angles = new List<int>();

            for (int i = 0; i < halfArray.GetLength(0); i++)
            {
                for (int j = 0; j < halfArray.GetLength(1); j++)
                {
                    if ((halfArray[i, j] > level)&&(((j - 90) > -5 && (j - 90) < 5)))
                    {
                        Normals.Add(i);
                        Angles.Add(j);
                    }
                }
            }

            unsafe
            {

                for (var i = 0; i < Normals.Count; i++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        var sourceRow = (byte*)sourceScan0 + (y * sourceStride);

                        double p = Normals[i] - halfArray.GetLength(0) / 2;
                        double cos = sinCos[1, Angles[i]];
                        double sin = sinCos[0, Angles[i]];

                        int x = (int)(p / cos - y * sin / cos);

                        if (x < width && x >= 0)
                        {
                            sourceRow[x * resultPixelSize] = 0;
                            sourceRow[x * resultPixelSize + 1] = 0;
                            sourceRow[x * resultPixelSize + 2] = 255;
                        }

                    }
                }
            }

            source.UnlockBits(sourceData);
        }
        public static void PostTreatment(Bitmap source)
        {
            var width = source.Width;
            var height = source.Height;

            var sourceData = source.LockBits(new Rectangle(new System.Drawing.Point(0, 0), source.Size),
                ImageLockMode.ReadWrite,
                source.PixelFormat);



            var sourceStride = sourceData.Stride;

            var sourceScan0 = sourceData.Scan0;

            var resultPixelSize = sourceStride / width;


            unsafe
            {
                for (var y = 1; y < height - 1; y++)
                {
                    var sourceRow = (byte*)sourceScan0 + (y * sourceStride);

                    for (var x = 1; x < width - 1; x++)
                    {


                        if (sourceRow[x * resultPixelSize] == 0)
                        {
                            for (int i = -1; i <= 1; i++)
                            {
                                var selectRow = (byte*)sourceScan0 + ((y + i) * sourceStride);
                                for (int j = -1; j <= 1; j++)
                                {
                                    if (selectRow[(x + j) * resultPixelSize] == 0 ||
                                        selectRow[(x + j) * resultPixelSize] == 255) break;

                                    if (x + j <= 0 || y + i <= 0 || y + i >= height - 1 || x + j >= width - 1) break;

                                    selectRow[(x + j) * resultPixelSize] = 0;
                                    selectRow[(x + j) * resultPixelSize + 1] = 0;
                                    selectRow[(x + j) * resultPixelSize + 2] = 0;
                                }
                            }
                        }
                        else
                        {
                            sourceRow[x * resultPixelSize] = 255;
                            sourceRow[x * resultPixelSize + 1] = 255;
                            sourceRow[x * resultPixelSize + 2] = 255;
                        }

                    }

                }
            }


            source.UnlockBits(sourceData);
        }
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