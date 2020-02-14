using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace som_cluster.Bean
{
    class Pretreatment
    {
        Bitmap bitmap;
        int[,,] color;

        public void Gray() {
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    color[0, x, y] = bitmap.GetPixel(x, y).R;
                    color[1, x, y] = bitmap.GetPixel(x, y).R;
                    color[2, x, y] = bitmap.GetPixel(x, y).R;
                }
        }
    
        public Pretreatment(Bitmap orignalBitamp) {
            this.bitmap = orignalBitamp;
            color = new int[3, orignalBitamp.Width, orignalBitamp.Height];

            for (int x = 0; x < orignalBitamp.Width; x++)
                for (int y = 0; y < orignalBitamp.Height; y++)
                {
                    color[0, x, y] = orignalBitamp.GetPixel(x, y).R;
                    color[1, x, y] = orignalBitamp.GetPixel(x, y).G;
                    color[2, x, y] = orignalBitamp.GetPixel(x, y).B;
                }
        }

        public Bitmap getBitmap()
        {
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(color[0, x, y], color[1, x, y], color[2, x, y]));
                }
            return bitmap;
        }

    }
}
