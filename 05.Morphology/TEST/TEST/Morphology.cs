/****************************************
PROGRAMER:공영재
PROJECT: Morphology
GROUP: https://hunter95001.github.io/crystalfox.github.io/
EXPLANATION:

Morphology
영상의 밝은 영역이나 어두운 영역을 축소, 확대하는 기법

침식
3*3 마스크를 사용하여 마스크에 대응하는 픽셀을 축소시키는 기법
입력 픽셀의 이웃에 있는 모든 픽셀 중 최솟값이 출력 픽셀의 값이 됩니다. 이진 영상의 경우, 값이 0으로 설정된 픽셀이 하나라도 있으면 출력 픽셀은 0으로 설정

팽창
3*3 마스크를 사용하여 마스크에 대응하는 픽셀을 증감시키는 기법
입력 픽셀의 이웃에 있는 모든 픽셀 중 최댓값이 출력 픽셀의 값이 됩니다. 이진 영상의 경우, 값이 1로 설정된 픽셀이 하나라도 있으면 출력 픽셀은 1로 설정

Edge
침식된 영역과 팽창된 영역은 서로 다르기 때문에 이를 X-OR연산을 사용하면 윤곽선 부분을 추출할 수가 있다.

출처
https://kr.mathworks.com/help/images/morphological-dilation-and-erosion.html
****************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    class Morphology
    {
        //침식
        public Bitmap Erosion(Bitmap bitmap)
        {
            Bitmap ObserverBitmap = bitmap;

            int[,] erosion_color = new int[bitmap.Width, bitmap.Height];
            int[,] color = new int[bitmap.Width, bitmap.Height];
            for (int x = 0; x < erosion_color.GetLength(0); x++)
                for (int y = 0; y < erosion_color.GetLength(1); y++)
                {
                    erosion_color[x, y] = bitmap.GetPixel(x, y).R;
                    color[x, y] = erosion_color[x, y];
                }
            for (int x = 1; x < erosion_color.GetLength(0) - 1; x++)
                for (int y = 1; y < erosion_color.GetLength(1) - 1; y++)
                {
                    for (int maskx = -1; maskx < 2; maskx++)
                        for (int masky = -1; masky < 2; masky++)
                        {
                            if (color[x + maskx, y + masky] == 255)
                                ObserverBitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        }
                }

            return ObserverBitmap;
        }
        //팽창
        public Bitmap Dilation(Bitmap bitmap)
        {
            Bitmap ObserverBitmap = new Bitmap(bitmap);

            int[,] dliation_color = new int[bitmap.Width, bitmap.Height];
            int[,] color = new int[bitmap.Width, bitmap.Height];
            for (int x = 0; x < dliation_color.GetLength(0); x++)
                for (int y = 0; y < dliation_color.GetLength(1); y++)
                {
                    dliation_color[x, y] = bitmap.GetPixel(x, y).R;
                    color[x, y] = dliation_color[x, y];
                }
            for (int x = 1; x < dliation_color.GetLength(0) - 1; x++)
                for (int y = 1; y < dliation_color.GetLength(1) - 1; y++)
                {
                    for (int maskx = -1; maskx < 2; maskx++)
                        for (int masky = -1; masky < 2; masky++)
                        {
                            if (color[x + maskx, y + masky] == 0)
                                ObserverBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        }
                }

            return ObserverBitmap;
        }

        //X-OR윤곽선 추출
        public Bitmap Edeg(Bitmap bitmap)
        {
            Bitmap ObserverBitmap = new Bitmap(bitmap);
            Bitmap ero = Erosion(bitmap);
            Bitmap dil = Dilation(bitmap);
            int[,] eroColor = new int[dil.Width, dil.Height];
            int[,] dilColor = new int[ero.Width, ero.Height];

            for (int x = 0; x < ero.Width; x++)
                for (int y = 0; y < ero.Height; y++)
                {
                    dilColor[x, y] = dil.GetPixel(x, y).R;
                    eroColor[x, y] = ero.GetPixel(x, y).R;
                }

            for (int x = 0; x < eroColor.GetLength(0); x++)
                for (int y = 0; y < eroColor.GetLength(1); y++)
                {
                    if (dilColor[x, y] != eroColor[x, y])
                        ObserverBitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    else
                        ObserverBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                }
            return ObserverBitmap;
        }
    }
}
