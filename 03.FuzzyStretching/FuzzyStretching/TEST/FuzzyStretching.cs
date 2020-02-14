using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    /*
     * 대비가 뚜렷하지 않으면 전처리하는 과정에서 그 해당 부분을
     * 잡음이라고 간주하고 해당 부분이 사라지게 된다.
     * 그러한 것을 방지하기 위해서 대비를 주는 목적이다.
     * 대비가 뚜렷하지 않은 영상은 특정 픽셀값에 몰려있다.
     * 하지만 대비가 뚜렷한 영상은 픽셀값들이 전반적으로 골고루 퍼져있다.
     * 몰려있는 명암도를 전반적으로 골고루 펴뜨려 주는 방식이 스트레칭이다.
     * 그 중에서 퍼지 논리, 퍼지 추론 방식을 이용해서 스트레칭 적용하는 것이
     * 퍼지 스트레칭이다.
    */

    class FuzzyStretching
    {

        public int[,] histogram(int[,] color)
        {
            int alpha = 0, beta = 255;
            foreach (var i in color)
            {
                alpha = Math.Max(alpha, i); // 상한
                beta = Math.Min(beta, i);   // 하한
            }

            double scale = 255.0 / (color.GetLength(0) * color.GetLength(1));

            for (int x = 0; x < color.GetLength(0); x++)  // 각 픽셀의 종류별 count
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    color[x, y] = (color[x, y] - alpha) * 255 / (beta - alpha);
                }

            for (int x = 0; x < color.GetLength(0); x++)  // 각 픽셀의 종류별 count
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    color[x, y] = Math.Abs(color[x, y] - 255);
                }

            return color;
        }

        // https://m.blog.naver.com/PostView.nhn?blogId=dlwjddns5&logNo=220687524811&proxyReferer=https%3A%2F%2Fwww.google.com%2F
        public int[,] equalization(int[,] color)
        {
            int alpha = 0, beta = 255;
            int[] histogram = new int[256];
            int[] sum_histogram = new int[256];
            int[] color_histogram = new int[256];
            int total = color.GetLength(0) * color.GetLength(1);
            int sum = 0;

            foreach (var i in color)
            {
                histogram[i]++;
                alpha = Math.Max(alpha, i); // 상한
                beta = Math.Min(beta, i);   // 하한
            }
            double scale = (double)alpha / total;

            for (int i = 0; i < 256; i++)
            {
                sum = sum + histogram[i];
                sum_histogram[i] = sum;
                color_histogram[i] = (int)(sum_histogram[i] * scale);
            }

            for (int x = 0; x < color.GetLength(0); x++)
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    color[x, y] = color_histogram[color[x, y]];
                }

            return color;
        }




        public Bitmap StretchingUsingFuzzy(Bitmap orig_bit)
        {
            // 전체 명암도의 평균을 구한다.
            Rectangle r = new Rectangle(new Point(0, 0), orig_bit.Size);
            BitmapData orgLock = orig_bit.LockBits(r, ImageLockMode.ReadWrite, orig_bit.PixelFormat);

            // 비트맵 이미지를 메모리에서 가져온다.
            int size = orig_bit.Size.Height * orgLock.Stride;
            byte[] bitmapArr = new byte[size];
            Marshal.Copy(orgLock.Scan0, bitmapArr, 0, size);
            // 조작하기

            //  Console.WriteLine("bit[{0}] = {1}", orig_bit.Width * 100 * 4 + 150 * 4,  bitmapArr[orig_bit.Width * 100 * 4 + 150 * 4]);

            int brightMax = 0;
            int brightMin = 255;

            // 평균 명암도 구하는 식
            int sum = 0;
            for (int i = 0; i < size; i += 4)
            {
                sum += bitmapArr[i];
                brightMax = Math.Max(brightMax, bitmapArr[i]);
                brightMin = Math.Min(brightMin, bitmapArr[i]);
            }
            double avgBright = sum / (orig_bit.Width * orig_bit.Height);
            // Console.WriteLine("max = {0}, min = {1}", brightMax, brightMin);
            // 밝은 픽셀값 거리 구하는 식 식(2)

            double Dmax = Math.Abs(brightMax - avgBright);
            double Dmin = Math.Abs(avgBright - brightMin);

            double adjustment = avgBright;
            // 식(2)에서 구한 값을 이용해 밝기의 조정률 결정
            if (avgBright > 128) adjustment = 255 - avgBright;
            else if (avgBright <= brightMin) adjustment = Dmin;
            else if (avgBright >= Dmax) adjustment = Dmax;

            // 최소 밝기값과 최대 밝기 값을 설정 후 삼각형 소속함수에 적용
            double imaximum = avgBright + adjustment;
            double iminimum = avgBright - adjustment;
            double imedium = (imaximum + iminimum) / 2;

            double L = iminimum, M = imedium, H = imaximum;
            // belong 0 ~ 51/2, 51/2 ~ 51, 51 ~ 102/2, 102/2 ~ 255
            double belong1 = 0, belong2 = 0, belong3 = 0, belong4 = 0;
            double belong1Max = 0, belong2Max = 0, belong3Max = 0, belong4Max = 0;


            for (int i = 0; i < bitmapArr.Length; i += 4)
            {
                // 퍼지 추론 적용
                double bL = 0, bM = 0, bH;
                int pixel = bitmapArr[i];

                if (pixel < M)
                {
                    // 낮은 구간에서의 소속함수
                    if (pixel <= L) bL = 1;
                    else if (pixel > L && pixel < M) bL = (M - pixel) / (M - L);
                    else bL = 0;



                    // 중간 이하 구간에서의 소속 함수
                    if (pixel <= M) bM = 0;
                    else if (pixel > L && pixel < M) bM = (pixel - L) / (M - L);


                    belong1 = Math.Min(bM, bL); //  u1 = 0.05

                    // (L, L, A 소속도)
                    belong1 = Math.Max(belong1, bL); // u1 = 0.94

                    // Low 구간에서 소속도를 0으로 잡고
                    // 그것보다 크면 u1Max에 저장 (u1의 소속도가 가장 높은 값을 선정한다.) 
                    //belong1Max = Math.Max(belong1Max, belong1);
                    if (belong1Max < belong1) belong1Max = belong1;
                    //if (u1max < u1) u1max = u1;

                    /* B 소속도 최대치 구하는 과정 */
                    // Imin -> Imid1 으로 올라가는 소속도 구하기.
                    belong2 = Math.Min(bL, bM); // 0.94, 0.05(선택)

                    // 위에서 뽑은 u2와 i_min 값 중 중간 소속도가 큰 것 고르기
                    belong2 = Math.Max(belong2, bM);
                    // (u2의 소속도가 가장 높은 값을 선정한다.) 
                    //belong2Max = Math.Max(belong2Max, belong1);
                    if (belong2Max < belong2) belong2Max = belong2;

                }

                else
                {
                    // 중간 이상 구간에서의 소속함수
                    if (pixel == M) bM = 1;
                    else bM = (H - pixel) / (H - M);


                    // 높은 구간에서의 소속함수
                    if (pixel > M && pixel < H) bH = (pixel - M) / (H - M);
                    else bH = 1;

                    belong3 = Math.Min(bM, bH);
                    belong3 = Math.Max(belong3, bM);
                    if (belong3Max < belong3) belong3Max = belong3;

                    //C최대치
                    // IMax 범위 벗어나면 소속도는 1이 된다.
                    // C 카테고리의 Imid2< pixel < Imax 소속도 구함(uh)
                    // 이 범위 벗어나면 매우 밝은 층에 속하므로 1이됨.
                    belong4 = Math.Min(bM, bH);
                    belong4 = Math.Max(belong4, bH);
                    if (belong4Max < belong4) belong4Max = belong4;
                }

            }

            // 무게 중심법
            double weightT1 = 0, weightB1 = 0, weightT2 = 0, weightB2 = 0;
            for (int pixel = 0; pixel < M; pixel++)
            {
                double bL = 0, bM = 0;
                if (pixel < (L + M) / 2.0)
                {
                    bL = Math.Min((M - pixel) / (M - L), 1.0);
                    weightT1 += bL * pixel;
                    weightB1 += bL;
                }
                else
                {
                    bM = Math.Min((pixel - L) / (M - L), 1.0);
                    weightT1 += bM * pixel;
                    weightB1 += bM;
                }
            }

            for (int pixel = (int)M; pixel < 256; pixel++)
            {
                double bM = 0, bH = 0;
                if (pixel < (M + H) / 2.0)
                {
                    bM = Math.Min((H - pixel) / (H - M), 1.0);
                    weightT2 += bM * pixel;
                    weightB2 += bM;
                }

                else
                {
                    bH = Math.Min((pixel - M) / (H - M), 1.0);
                    weightT2 += bH * pixel;
                    weightB2 += bH;
                }

            }

            int alpha = (int)(weightT1 / weightB1); // 하한
            int beta = (int)(weightT2 / weightB2); // 상한





            // 소속함수에서 구해진 소속도에 a_cut 적용하여 하한과 상한 구하기
            // 하한: 아래쪽의 한계, 상한: 위쪽의 한계

            /*
            // acut 이상인 X 픽셀값 중에서 가장 높은 것을 상한
            // 가장 낮은 것을 하한
            double a_cut = 0.5;
            if (iminimum != 0) a_cut = imedium / imaximum;

            */


            byte[] lut = new byte[256];

            for (int i = beta; i < 256; i++)
            {
                lut[i] = 255;
            }


            for (int i = alpha; i < beta; i++)
            {
                double ff = (i - alpha) * 255.0 / (beta - alpha);
                lut[i] = (byte)ff;
            }
            int noCount = 5;

            int wid_heig_fuzzy = orig_bit.Width * orig_bit.Height;
            int wid_heig_no = orig_bit.Width * orig_bit.Height;

            double fuzzyStSum = 0, noSum = 0;
            for (int i = 0; i < bitmapArr.Length; i += 4)
            {
                if (bitmapArr[i] <= noCount)
                    wid_heig_no--;
                else
                    noSum += bitmapArr[i];

                bitmapArr[i] = lut[bitmapArr[i]];
                bitmapArr[i + 1] = lut[bitmapArr[i + 1]];
                bitmapArr[i + 2] = lut[bitmapArr[i + 2]];

                if (bitmapArr[i] <= noCount)
                    wid_heig_fuzzy--;
                else
                    fuzzyStSum += bitmapArr[i];
            }
            double fuzzyAvg = fuzzyStSum / (wid_heig_fuzzy);
            double noAvg = noSum / wid_heig_no;
            //  Console.WriteLine("pre fuzzy avg = {0}, post fuzzy = {1}, post - pre = {2}",  noAvg, fuzzyAvg, fuzzyAvg - noAvg);
            Marshal.Copy(bitmapArr, 0, orgLock.Scan0, bitmapArr.Length);
            orig_bit.UnlockBits(orgLock);
            return orig_bit;
        }

    }
}
