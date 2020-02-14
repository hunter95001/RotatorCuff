using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace som_cluster
{
    class SOM
    {
        double[] Weight;
        double[] input;
        Bitmap bitmap;
        int rec;
        public SOM(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            Weight = new double[bitmap.Width*bitmap.Height];
            input = new double[bitmap.Width * bitmap.Height];
            int R;
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    R=bitmap.GetPixel(x, y).R;
                    input[rec++] = R;
                }
            Console.WriteLine(bitmap.Width * bitmap.Height);
            for (int i = 0; i < input.Length; i++)
            {
                Console.WriteLine(input[i]);
            }
            

            Random ran = new Random();
      
                    //Weight[i, j] = ran.NextDouble() * 255.0;

        }
    }
}
