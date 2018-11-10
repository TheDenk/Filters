namespace ImageManipulate
{
    public struct Matrix
    {
        public static double[,] Nulls
        {
            get
            {
                return new double[,] {
                    { 0,  0,  0  },
                    { 0,  0,  0  },
                    { 0,  0,  0  }};
            }
        }
        public static double[,] PrewittGorizontal => new double[,] {
            { -1,  -1,  -1  },
            { 0,  0,  0  },
            { 1,  1,  1  }};

        public static double[,] PrewittVertical
        {
            get
            {
                return new double[,] {
                    { -1,  0,  1  },
                    { -1,  0,  1  },
                    { -1,  0,  1  }};
            }
        }
        public static double[,] Kirsch3x3Gorizontal
        {
            get
            {
                return new double[,]
                { {  5, -3, -3, },
                    {  5,  0, -3, },
                    {  5, -3, -3, }, };
            }
        }
        public static double[,] Kirsch3x3Vertical
        {
            get
            {
                return new double[,]
                { {  5,  5,  5, },
                    { -3,  0, -3, },
                    { -3, -3, -3, }, };
            }
        }
        public static double[,] SobelGorizontal
        {
            get
            {
                return new double[,] {
            { -1,  -2,  -1  },
            { 0,  0,  0  },
            { 1,  2,  1  }
            };
            }
        }
        public static double[,] SobelVertical
        {
            get
            {
                return new double[,] {
                { -1,  0,  1  },
                { -2,  0,  2  },
                { -1,  0,  1  }};
            }
        }
        // koef 1.0 / 9.0
        public static double[,] Mean3x3
        {
            get
            {
                return new double[,]
               { {  1, 1, 1, },
                  {  1, 1, 1, },
                  {  1, 1, 1, }, };
            }
        }
        // koef  1.0 / 25.0
        public static double[,] Mean5x5
        {
            get
            {
                return new double[,]
               { {  1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1 }, };
            }
        }
        // koef  1.0 / 49.0
        public static double[,] Mean7x7
        {
            get
            {
                return new double[,]
                { {  1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1 }, };
            }
        }
        // koef  1.0 / 81.0
        public static double[,] Mean9x9
        {
            get
            {
                return new double[,]
                { {  1, 1, 1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1, 1, 1 },
                  {  1, 1, 1, 1, 1, 1, 1, 1, 1 }, };
            }
        }
        public static double[,] Laplacian3x3
        {
            get
            {
                return new double[,]
                { { -1, -1, -1, },
                    { -1,  8, -1, },
                    { -1, -1, -1, }, };
            }
        }
        public static double[,] Laplacian5x5
        {
            get
            {
                return new double[,]
                { { -1, -1, -1, -1, -1, },
                    { -1, -1, -1, -1, -1, },
                    { -1, -1, 24, -1, -1, },
                    { -1, -1, -1, -1, -1, },
                    { -1, -1, -1, -1, -1  } };
            }
        }
        public static double[,] LaplacianOfGaussian
        {
            get
            {
                return new double[,]
                { {  0,  0, -1,  0,  0 },
                    {  0, -1, -2, -1,  0 },
                    { -1, -2, 16, -2, -1 },
                    {  0, -1, -2, -1,  0 },
                    {  0,  0, -1,  0,  0 } };
            }
        }
        // koef 1d/16.0d
        public static double[,] Gauss3x3
        {
            get
            {
                return new double[,] {
                {  1,  2,  1  },
                {  2,  4,  2  },
                {  1,  2,  1  }
            };
            }
        }
        // koef  1d/159.0d
        public static double[,] Gauss5x5
        {
            get
            {
                return new double[,] {
                {  2,  4,  5,   4,  2  },
                {  4,  9,  12,   9,  4  },
                {  5,  12,  15,   12,  5  },
                {  4,  9,  12,   9,  4  },
                {  2,  4,  5,   4,  2  }};
            }
        }
        // 1.0 / 10.0
        public static double[,] MotionBlur5x5
        {
            get
            {
                return new double[,]
                { {  1, 0, 0, 0, 1 },
                  {  0, 1, 0, 1, 0 },
                  {  0, 0, 1, 0, 0 },
                  {  0, 1, 0, 1, 0 },
                  {  1, 0, 0, 0, 1 }, };
            }
        }
        //1.0 / 5.0
        public static double[,] MotionBlur5x5At45Degrees
        {
            get
            {
                return new double[,]
                { {  0, 0, 0, 0, 1 },
                  {  0, 0, 0, 1, 0 },
                  {  0, 0, 1, 0, 0 },
                  {  0, 1, 0, 0, 0 },
                  {  1, 0, 0, 0, 0 }, };
            }
        }
        //1.0 / 5.0
        public static double[,] MotionBlur5x5At135Degrees
        {
            get
            {
                return new double[,]
                { {  1, 0, 0, 0, 0 },
                  {  0, 1, 0, 0, 0 },
                  {  0, 0, 1, 0, 0 },
                  {  0, 0, 0, 1, 0 },
                  {  0, 0, 0, 0, 1 }, };
            }
        }
        //1.0 / 14.0
        public static double[,] MotionBlur7x7
        {
            get
            {
                return new double[,]
                { {  1, 0, 0, 0, 0, 0, 1 },
                  {  0, 1, 0, 0, 0, 1, 0 },
                  {  0, 0, 1, 0, 1, 0, 0 },
                  {  0, 0, 0, 1, 0, 0, 0 },
                  {  0, 0, 1, 0, 1, 0, 0 },
                  {  0, 1, 0, 0, 0, 1, 0 },
                  {  1, 0, 0, 0, 0, 0, 1 }, };
            }
        }
        // 1.0 / 7.0
        public static double[,] MotionBlur7x7At45Degrees
        {
            get
            {
                return new double[,]
                { {  0, 0, 0, 0, 0, 0, 1 },
                  {  0, 0, 0, 0, 0, 1, 0 },
                  {  0, 0, 0, 0, 1, 0, 0 },
                  {  0, 0, 0, 1, 0, 0, 0 },
                  {  0, 0, 1, 0, 0, 0, 0 },
                  {  0, 1, 0, 0, 0, 0, 0 },
                  {  1, 0, 0, 0, 0, 0, 0 }, };
            }
        }
        // 1.0 / 7.0
        public static double[,] MotionBlur7x7At135Degrees
        {
            get
            {
                return new double[,]
                { {  1, 0, 0, 0, 0, 0, 0 },
                  {  0, 1, 0, 0, 0, 0, 0 },
                  {  0, 0, 1, 0, 0, 0, 0 },
                  {  0, 0, 0, 1, 0, 0, 0 },
                  {  0, 0, 0, 0, 1, 0, 0 },
                  {  0, 0, 0, 0, 0, 1, 0 },
                  {  0, 0, 0, 0, 0, 0, 1 }, };
            }
        }
        //1.0 / 18.0
        public static double[,] MotionBlur9x9
        {
            get
            {
                return new double[,]
                { { 1, 0, 0, 0, 0, 0, 0, 0, 1, },
                  { 0, 1, 0, 0, 0, 0, 0, 1, 0, },
                  { 0, 0, 1, 0, 0, 0, 1, 0, 0, },
                  { 0, 0, 0, 1, 0, 1, 0, 0, 0, },
                  { 0, 0, 0, 0, 1, 0, 0, 0, 0, },
                  { 0, 0, 0, 1, 0, 1, 0, 0, 0, },
                  { 0, 0, 1, 0, 0, 0, 1, 0, 0, },
                  { 0, 1, 0, 0, 0, 0, 0, 1, 0, },
                  { 1, 0, 0, 0, 0, 0, 0, 0, 1, }, };
            }
        }
        // 1.0 / 9.0
        public static double[,] MotionBlur9x9At45Degrees
        {
            get
            {
                return new double[,]
                { { 0, 0, 0, 0, 0, 0, 0, 0, 1, },
                  { 0, 0, 0, 0, 0, 0, 0, 1, 0, },
                  { 0, 0, 0, 0, 0, 0, 1, 0, 0, },
                  { 0, 0, 0, 0, 0, 1, 0, 0, 0, },
                  { 0, 0, 0, 0, 1, 0, 0, 0, 0, },
                  { 0, 0, 0, 1, 0, 0, 0, 0, 0, },
                  { 0, 0, 1, 0, 0, 0, 0, 0, 0, },
                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, },
                  { 1, 0, 0, 0, 0, 0, 0, 0, 0, }, };
            }
        }
        // 1.0 / 9.0
        public static double[,] MotionBlur9x9At135Degrees
        {
            get
            {
                return new double[,]
                { { 1, 0, 0, 0, 0, 0, 0, 0, 0, },
                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, },
                  { 0, 0, 1, 0, 0, 0, 0, 0, 0, },
                  { 0, 0, 0, 1, 0, 0, 0, 0, 0, },
                  { 0, 0, 0, 0, 1, 0, 0, 0, 0, },
                  { 0, 0, 0, 0, 0, 1, 0, 0, 0, },
                  { 0, 0, 0, 0, 0, 0, 1, 0, 0, },
                  { 0, 0, 0, 0, 0, 0, 0, 1, 0, },
                  { 0, 0, 0, 0, 0, 0, 0, 0, 1, }, };
            }
        }


    }
}

