/****************************************
PROGRAMER:공영재
PROJECT: ROI
EXPLANATION: 

이진화란 
0~255의 색상 값으로 이루어져있는 비트맵을 0과 1사이 값으로 조절하는 기법
     
Gonzalez 이진화
Step#1. Gonzalez 기법은 ROI영역에서 최대 밝기값과 최소 밝기 값의 중간 값을 초기 임계값 T로 추정한다. 
Step#2. 초기 임계값보다 큰 영역을 G1, 초기 영역 보다 작은 영역을 G2로 나눈다. 
Step#3. G1 영역의 평균값을 , G2 영역의 평균값을  계산한다. 계산된 와  통해 새로운 경계 값을 설정한다. 
Step#4. 이전 경계값과 새로 설정된 경계값이 같거나 오차보다 작을 때 까지 반복하여 경계값을 설정 한다.

Horizontal 이진화
가로축의 이미지 명암도에 평균값으로 임계값을 설정하고 그 임계값을 기준으로 명암도를 구분한다. 

Vertical 이진화  
세로축의 이미지 명암도에 대해서 평균값으로 임계값을 설정하고 임계값을 기준으로 명암도를 구분한다. 

Gonzalez_Horizontal_Vertical
곤잘레즈 Horizontal Vertical AND 연산을 적용한다.
****************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    class Binarization
    {
        Bitmap ObserverBitmap;
        int[,] color;
        public Binarization(Bitmap bitmap)
        {
            ObserverBitmap = new Bitmap(bitmap);
            color = new int[bitmap.Width, bitmap.Height];
            for (int x = 0; x < color.GetLength(0); x++)
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    color[x, y] = ObserverBitmap.GetPixel(x, y).R;
                }

        }

        public Bitmap Vertical()
        {
            Bitmap VerticalBitmap = new Bitmap(ObserverBitmap);
            int[,] vertical_color = new int[color.GetLength(0), color.GetLength(1)];
            int[] sum = new int[color.GetLength(0)];
            foreach (var i in vertical_color)
            {
            }
            for (int x = 0; x < color.GetLength(0); x++)
            {
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    sum[x] = sum[x] + color[x, y];
                }
                sum[x] = sum[x] / color.GetLength(0);
            }


            for (int x = 0; x < color.GetLength(0); x++)
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    vertical_color[x, y] = color[x, y] < sum[x] ? 0 : 255;
                    VerticalBitmap.SetPixel(x, y, Color.FromArgb(vertical_color[x, y], vertical_color[x, y], vertical_color[x, y]));

                }

            return VerticalBitmap;
        }

        public Bitmap Horizontal()
        {
            Bitmap HorizontalBitmap = new Bitmap(ObserverBitmap);
            int[,] Horizontal_color = new int[color.GetLength(0), color.GetLength(1)];
            foreach (var i in Horizontal_color)
            {
            }
            int[] sum = new int[color.GetLength(1)];
            for (int y = 0; y < color.GetLength(1); y++)
            {
                for (int x = 0; x < color.GetLength(0); x++)
                {
                    sum[y] = sum[y] + color[x, y];
                }
                sum[y] = sum[y] / color.GetLength(0);
            }
            for (int x = 0; x < color.GetLength(0); x++)
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    Horizontal_color[x, y] = color[x, y] < sum[y] ? 0 : 255;
                    HorizontalBitmap.SetPixel(x, y, Color.FromArgb(Horizontal_color[x, y], Horizontal_color[x, y], Horizontal_color[x, y]));
                }


            return HorizontalBitmap;
        }

        public Bitmap Gonzalez()
        {
            Bitmap GonzalezBitmap = new Bitmap(ObserverBitmap);
            int threshold;
            int pastthreshold;
            int G1 = 0; //임계값 보다 큰 영역
            int G1count = 0;
            int u1 = 0; //
            int G2 = 0; //임계값 보다 작은 영역
            int G2count = 0;
            int u2 = 0;
            int max = 0;
            int min = 999;

            int[,] Gonzalez_color = new int[color.GetLength(0), color.GetLength(1)];
            bool option = true;

            for (int x = 0; x < color.GetLength(0); x++)
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    if (max < color[x, y])
                    {
                        max = color[x, y];
                    }
                    else if (min > color[x, y])
                    {
                        min = color[x, y];
                    }
                }
            threshold = (max + min) / 2;


            do
            {
                G1 = 0; //임계값 보다 큰 영역
                G1count = 0;
                u1 = 0; //
                G2 = 0; //임계값 보다 작은 영역
                G2count = 0;
                u2 = 0;



                for (int x = 0; x < color.GetLength(0); x++)
                    for (int y = 0; y < color.GetLength(1); y++)
                    {
                        if (color[x, y] > threshold)
                        {
                            G1 += color[x, y];
                            G1count++;
                        }
                        else if (color[x, y] < threshold)
                        {
                            G2 += color[x, y];
                            G2count++;
                        }
                    }
                //Console.WriteLine(G1 + " G1 " + G1count + " " + u1);
                //Console.WriteLine(G2 + " G2 " + G2count + " " + u2);
                u1 = G1 / G1count;
                u2 = G2 / G2count;
                pastthreshold = threshold;
                threshold = (u1 + u2) / 2;

                if (threshold == pastthreshold)
                    option = false;

                //Console.WriteLine(G1 + " G1 " + G1count + " " + u1);
                //Console.WriteLine(G2 + " G2 " + G2count + " " + u2);
                //Console.WriteLine(threshold + " 이전 " + pastthreshold);
            } while (option);

            for (int x = 0; x < color.GetLength(0); x++)
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    Gonzalez_color[x, y] = color[x, y] < threshold ? 0 : 255;
                    GonzalezBitmap.SetPixel(x, y, Color.FromArgb(Gonzalez_color[x, y], Gonzalez_color[x, y], Gonzalez_color[x, y]));

                }
            return GonzalezBitmap;
        }

        public Bitmap Gonzalez_Horizontal_Vertical()
        {
            Bitmap Horizontal_Bitmap = Horizontal();
            Bitmap Gonzalez_Bitmap = Gonzalez();
            Bitmap Vertica_Bitmap = Vertical();
            int[,] Horizontal_Color = new int[color.GetLength(0), color.GetLength(1)];
            int[,] Gonzalez_Color = new int[color.GetLength(0), color.GetLength(1)];
            int[,] Vertical_Color = new int[color.GetLength(0), color.GetLength(1)];

            for (int x = 0; x < color.GetLength(0); x++)
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    Horizontal_Color[x, y] = Horizontal_Bitmap.GetPixel(x, y).R;
                    Gonzalez_Color[x, y] = Gonzalez_Bitmap.GetPixel(x, y).R;
                    Vertical_Color[x, y] = Vertica_Bitmap.GetPixel(x, y).R;
                }

            for (int x = 0; x < color.GetLength(0); x++)
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    if (Horizontal_Color[x, y] == 255 && Gonzalez_Color[x, y] == 255 && Vertical_Color[x, y] == 255)
                        ObserverBitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    else
                        ObserverBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                }
            return ObserverBitmap;

        }

    }
}
