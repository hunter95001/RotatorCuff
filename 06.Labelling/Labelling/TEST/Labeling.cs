/****************************************
PROGRAMER:공영재
PROJECT: Labeling
GROUP: https://hunter95001.github.io/crystalfox.github.io/
EXPLANATION: 
라벨링은 도형에 번호를 매겨 사용하는 방법으로 8방향 탐색 기법을 적용한다.
라벨링은 이진화 된 사진을 사용하여야 한다.
Step #1 8방향 윤곽선 탐색
Step #2 Tagging은 라벨링된 작업을 처리하는 역할을 합니다.
         2.1 라벨의 개수와 라벨 번호의 변수를 설정
         2.2 제대로 라벨링 되지 않은 라벨번호때문에 라벨 번호가 바뀌지 않을떄까지 라벨링 해줍니다.
         2.3 5% 이하의 라벨 번호는 삭제
         2.4 도형이 큰 순서대로 정렬 [개수가 많은 순서대로, 라벨 번호도 바꿔줘야 합니다.]
         2.5 회전근개 아랫 부분을 찾아야함.
         2.6 라벨링 영역중 가장큰 영역과 2번째로 큰 영역
****************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace TEST
{
    class Labeling
    {
        Bitmap ObserverBitmap;
        int[,] label;       //라벨링 번호가 담겨져 있는 배열
        int[,] color;       // 컬러 색상이 담겨져 있는 배열
        int[] tag;          // 색상 번호가 담겨져 있는 번호
        int count = 0;
        int value = 20;     // 잡음을 제거하기 위해서 몇퍼센트인지 판단. [int라 /20 =5%]
        //생성자는 초기화를 담당합니다.
        public Labeling(Bitmap bitmap)
        {
            ObserverBitmap = new Bitmap(bitmap);
            label = new int[bitmap.Width, bitmap.Height];
            color = new int[bitmap.Width, bitmap.Height];
            label.Initialize();
            for (int y = 0; y < bitmap.Height; y++)
                for (int x = 0; x < bitmap.Width; x++)
                {
                    color[x, y] = bitmap.GetPixel(x, y).R;
                }
        }
        //실행되는 부분.
        public void Run()
        {
            Navigation();   //8방향 윤곽선 탐색 기법및 번호 부여한다.
            Tagging();      //인접한 도형의 번호로 바꾼다.
        }
        public void AIRun()
        {
            Navigation();   //8방향 윤곽선 탐색 기법및 번호 부여한다.
            AITagging();      //인접한 도형의 번호로 바꾼다.
        }
        #region 윤곽선 탐색 및 라벨 부여
        private void Navigation()
        {
            for (int y = 0; y < color.GetLength(1); y++)
                for (int x = 0; x < color.GetLength(0); x++)
                {
                    if (color[x, y] == 255 && label[x, y] == 0)
                    {
                        Detection8(x, y);   //8방향 탐색 기법
                    }
                }
        }
        /* 
            8방향 탐색 기법
            0.0을 기준으로
            -1,-1  0,-1 +1,-1
            -1, 0  0, 0 +1, 0
            -1,+1  0,+1 +1,+1
            탐색한다.
            단 비트맵의 4각형 가장자리는 예외 처리를 해줘서 처리하는 역할을 한다.
         */
        private void Detection8(int x, int y)
        {
            Boolean option = true;  //while문 종료 조건.
            count++;                //라벨 번호를 매기기 위해서
            //탐지 순서를 바꿀 경우 예외가 발생할 수 있습니다.
            while (option)
            {
                //좌측 상단 가장자리    -> x와 y값이 음수가 되는 것을 방지 합니다.
                if (x == 0 && y == 0) //3시 5시 6시
                {
                    if (color[x + 1, y] == 255 && label[x + 1, y] == 0)    //3시
                    {
                        x++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x + 1, y + 1] == 255 && label[x + 1, y + 1] == 0)    //5시
                    {
                        x++;
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x, y + 1] == 255 && label[x, y + 1] == 0)    //6시
                    {
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else
                    {
                        option = false;
                    }
                }
                //좌측 하단 가장자리    -> x가 음수, y가 사진 크기를 벗어나는 것을 방지합니다.
                else if (x == 0 && y == ObserverBitmap.Height - 1) //12시 1시 3시
                {
                    if (color[x, y - 1] == 255 && label[x, y - 1] == 0)    //12시
                    {
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x + 1, y - 1] == 255 && label[x + 1, y - 1] == 0)    //1시
                    {
                        x++;
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x + 1, y] == 255 && label[x + 1, y] == 0)    //3시
                    {
                        x++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else
                    {
                        option = false;
                    }
                }
                //우측 상단 가장자리    -> x가 사진 크기, y가 음수가 되는 것을 방지합니다.
                else if (x == ObserverBitmap.Width - 1 && y == 0) //6시 7시 9시
                {
                    if (color[x, y + 1] == 255 && label[x, y + 1] == 0)    //6시
                    {
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x - 1, y + 1] == 255 && label[x - 1, y + 1] == 0)    //7시
                    {
                        x--;
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x - 1, y] == 255 && label[x - 1, y] == 0)    //9시
                    {
                        x--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else
                    {
                        option = false;
                    }
                }
                //우측 하단 가장자리    -> x가 사진 크기, y가 사진 크기를 벗어나는 것을 방지합니다.
                else if (x == ObserverBitmap.Width - 1 && y == ObserverBitmap.Height - 1)//9시 11시 12시
                {
                    if (color[x - 1, y] == 255 && label[x - 1, y] == 0)    //9시
                    {
                        x--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x - 1, y - 1] == 255 && label[x - 1, y - 1] == 0)    //11시
                    {
                        x--;
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x, y - 1] == 255 && label[x, y - 1] == 0)    //12시
                    {
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else
                    {
                        option = false;
                    }
                }
                //상단                  -> y값이 음수가 되는 것을 방지합니다.
                else if (y == 0) //3시 5시 6시 7시 9시 
                {
                    if (color[x + 1, y] == 255 && label[x + 1, y] == 0)    //3시
                    {
                        x++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x + 1, y + 1] == 255 && label[x + 1, y + 1] == 0)    //5시
                    {
                        x++;
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x, y + 1] == 255 && label[x, y + 1] == 0)    //6시
                    {
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x - 1, y + 1] == 255 && label[x - 1, y + 1] == 0)    //7시
                    {
                        x--;
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x - 1, y] == 255 && label[x - 1, y] == 0)    //9시
                    {
                        x--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else
                    {
                        option = false;
                    }
                }
                //좌측                  -> x값이 음수가 되는 것을 방지합니다.
                else if (x == 0) //12시 1시 3시 5시 6시
                {
                    if (color[x, y - 1] == 255 && label[x, y - 1] == 0)    //12시
                    {
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x + 1, y - 1] == 255 && label[x + 1, y - 1] == 0)    //1시
                    {
                        x++;
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x + 1, y] == 255 && label[x + 1, y] == 0)    //3시
                    {
                        x++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x + 1, y + 1] == 255 && label[x + 1, y + 1] == 0)    //5시
                    {
                        x++;
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x, y + 1] == 255 && label[x, y + 1] == 0)    //6시
                    {
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x, y - 1] == 255 && label[x, y - 1] == 0)    //12시
                    {
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else
                    {
                        option = false;
                    }
                }
                //하단                  -> y값이 사진 크기를 벗어나는 것을 방지합니다.
                else if (y == ObserverBitmap.Height - 1) //9시 11시 12시 1시 3시
                {
                    if (color[x - 1, y] == 255 && label[x - 1, y] == 0)    //9시
                    {
                        x--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x - 1, y - 1] == 255 && label[x - 1, y - 1] == 0)    //11시
                    {
                        x--;
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x, y - 1] == 255 && label[x, y - 1] == 0)    //12시
                    {
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x + 1, y - 1] == 255 && label[x + 1, y - 1] == 0)    //1시
                    {
                        x++;
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x + 1, y] == 255 && label[x + 1, y] == 0)    //3시
                    {
                        x++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else
                    {
                        option = false;
                    }
                }
                //우측                  -> x값이 사진 크기를 벗어나는 것을 방지합니다.
                else if (x == ObserverBitmap.Width - 1) //6시 7시 9시 11시 12시
                {
                    if (color[x, y + 1] == 255 && label[x, y + 1] == 0)    //6시
                    {
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x - 1, y + 1] == 255 && label[x - 1, y + 1] == 0)    //7시
                    {
                        x--;
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x - 1, y] == 255 && label[x - 1, y] == 0)    //9시
                    {
                        x--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x - 1, y - 1] == 255 && label[x - 1, y - 1] == 0)    //11시
                    {
                        x--;
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x, y - 1] == 255 && label[x, y - 1] == 0)    //12시
                    {
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else
                    {
                        option = false;
                    }
                }
                //사진 내부에 있을 경우
                else
                {
                    if (color[x + 1, y] == 255 && label[x + 1, y] == 0)    //3시
                    {
                        x++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x, y + 1] == 255 && label[x, y + 1] == 0)    //6시
                    {
                        y++;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x - 1, y] == 255 && label[x - 1, y] == 0)    //9시
                    {
                        x--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else if (color[x, y - 1] == 255 && label[x, y - 1] == 0)    //12시
                    {
                        y--;
                        label[x, y] = count;
                        ObserverBitmap.SetPixel(x, y, Color.Red);
                    }
                    else
                    {
                        option = false;
                    }
                }
            }
        }
        #endregion
        #region Tagging, Convert
        /*
         Tagging은 라벨링된 작업을 처리하는 역할을 합니다.
         Step #1 라벨의 개수와 라벨 번호의 변수를 설정
         Step #2 제대로 라벨링 되지 않은 라벨번호때문에 라벨 번호가 바뀌지 않을떄까지 라벨링 해줍니다.
         Step #3 5% 이하의 라벨 번호는 삭제
         Step #4 도형이 큰 순서대로 정렬 [개수가 많은 순서대로, 라벨 번호도 바꿔줘야 합니다.]
         Step #5 회전근개 아랫 부분을 찾아야함.
         Step #6 라벨링 영역중 가장큰 영역과 2번째로 큰 영역
         */
        private void Tagging()
        {
            //Step #1 라벨의 개수와 라벨 번호의 변수를 설정
            int[] countarray = new int[count + 1];  //라벨링의 개수를 저장하는 변수
            tag = new int[count + 1];               //태그 개수는 = 카운트 개수 = 라벨링 도형의 개수

            for (int i = 0; i < countarray.Length; i++)
            {
                tag[i] = i;                         //태그 번호는 0,1,2,3,~....
            }
            countarray.Initialize();                //개수를 초기화 시켜줌.
            //Step #2 제대로 라벨링 되지 않은 라벨번호때문에 라벨 번호가 바뀌지 않을떄까지 라벨링 해줍니다.
            Console.WriteLine("covert 중.... 시간이 오래 걸립니다.");
            Convert(label);                         //Convert는 주위의 라벨링 번호를 바꿔 주는 역할을 한다.
            Console.WriteLine("covert 종료 ....");

            for (int y = 0; y < label.GetLength(1); y++)
                for (int x = 0; x < label.GetLength(0); x++)
                {
                    countarray[label[x, y]]++;      //라벨 개수를 저장함
                }
            countarray[0] = 0;                      //0은 도형이 없는 구간 이기 때문에 0번은 라벨링 되면 안됨

            //Step #3 5% 이하의 라벨 번호는 삭제
            int sum = 0;                            //도형의 윤곽선의 총 개수
            for (int i = 0; i < countarray.Length; i++)
            {
                sum += countarray[i];
            }
            sum = sum / value;                        // 5% 
            for (int y = 0; y < label.GetLength(1); y++)
                for (int x = 0; x < label.GetLength(0); x++)
                {
                    if (countarray[label[x, y]] < sum)  //5%보다 작으면 라벨의 번호를 지워라
                    {
                        label[x, y] = 0;
                    }
                }
            //Step #4 도형이 큰 순서대로 정렬 [개수가 많은 순서대로, 라벨 번호도 바꿔줘야 합니다.]
            sort(countarray, tag);                      //도형이 큰 순서대로 소팅하기 위해서 


            //Step #5 회전근개 아랫 부분을 찾아야함.
            Boolean endfor = true;                      // for문 종료하는 변수
            int lower = 0;                              // 회전근개 아래 부분 라벨링 번호 변수
            for (int y = label.GetLength(1) - 1; y > 0; y--)
            {
                for (int x = 200; x < 500; x++)         // 중심에서 벗어난 지역은 잡음으로 판단.
                {
                    if (label[x, y] != 0)
                    {
                        lower = label[x, y];            // 가장 먼저 만나는 번호를 저장
                        endfor = false;                 // y for문을 종료하기 위해서
                        break;                          // x for문 종료
                    }
                }
                if (endfor == false)
                {
                    break;                              // y for 문 종료
                }
            }

            //Step #6 라벨링 영역중 가장큰 영역과 2번째로 큰 영역
            for (int y = 0; y < label.GetLength(1); y++)
                for (int x = 0; x < label.GetLength(0); x++)
                {
                    try
                    {   //tag가 없을 경우 예외를 처리 해줍니다.
                        //회전근개 영역을 확인하기 위한 양자화 기법입니다.
                        if (lower == label[x, y])                       // 회전근개 아랫 부분 색칠
                            ObserverBitmap.SetPixel(x, y, Color.LightPink);
                        else if (tag[0] == label[x, y])                 // 가장 큰 영역 색칠
                            ObserverBitmap.SetPixel(x, y, Color.Blue);
                        else if (tag[1] == label[x, y])                 // 2번째로 큰 영역 색칠
                            ObserverBitmap.SetPixel(x, y, Color.Green);
                        else
                        {
                            ObserverBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        }
                        //실제로 사용할때는 아래 코드를 실행시켜주세요
                        //if (lower == label[x, y])                       // 회전근개 아랫 부분 색칠
                        //    ObserverBitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        //else if (tag[0] == label[x, y])                 // 가장 큰 영역 색칠
                        //    ObserverBitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        //else if (tag[1] == label[x, y])                 // 2번째로 큰 영역 색칠
                        //    ObserverBitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        //else
                        //{
                        //    ObserverBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        //}
                    }
                    catch { }//무시하고 처리함.
                }

        }
        private void AITagging()
        {
            //Step #1 라벨의 개수와 라벨 번호의 변수를 설정
            int[] countarray = new int[count + 1];  //라벨링의 개수를 저장하는 변수
            tag = new int[count + 1];               //태그 개수는 = 카운트 개수 = 라벨링 도형의 개수
            for (int i = 0; i < countarray.Length; i++)
            {
                tag[i] = i;                         //태그 번호는 0,1,2,3,~....
            }
            countarray.Initialize();                //개수를 초기화 시켜줌.
            //Step #2 제대로 라벨링 되지 않은 라벨번호때문에 라벨 번호가 바뀌지 않을떄까지 라벨링 해줍니다.
            Console.WriteLine("covert 중.... 시간이 오래 걸립니다.");
            Convert(label);                         //Convert는 주위의 라벨링 번호를 바꿔 주는 역할을 한다.
            Console.WriteLine("covert 종료 ....");
            for (int y = 0; y < label.GetLength(1); y++)
                for (int x = 0; x < label.GetLength(0); x++)
                {
                    countarray[label[x, y]]++;      //라벨 개수를 저장함
                }
            countarray[0] = 0;                      //0은 도형이 없는 구간 이기 때문에 0번은 라벨링 되면 안됨

            //Step #4 도형이 큰 순서대로 정렬 [개수가 많은 순서대로, 라벨 번호도 바꿔줘야 합니다.]
            sort(countarray, tag);                      //도형이 큰 순서대로 소팅하기 위해서 
            //Step #5 회전근개 아랫 부분을 찾아야함.
            Boolean endfor = true;                      // for문 종료하는 변수
            int lower = 0;                              // 회전근개 아래 부분 라벨링 번호 변수
            for (int y = label.GetLength(1) - 1; y > 0; y--)
            {
                for (int x = 200; x < 500; x++)         // 중심에서 벗어난 지역은 잡음으로 판단.
                {
                    if (label[x, y] != 0)
                    {
                        lower = label[x, y];            // 가장 먼저 만나는 번호를 저장
                        endfor = false;                 // y for문을 종료하기 위해서
                        break;                          // x for문 종료
                    }
                }
                if (endfor == false)
                {
                    break;                              // y for 문 종료
                }
            }

        }
        /*
         Convert는 인접해있는 도형의 라벨 번호는 최소 라벨번호를 따릅니다.
         Convert 작업은 더이상 라벨 번호가 바뀌지 않을떄까지 계속 작업 됩니다.
         예외 처리
         label의 번호가 0이면 도형이 아님으로 Max값을 부여합니다.
        */
        private void Convert(int[,] getlabel)
        {
            Boolean options = true;     //반복문 종료 조건
            int[] convert = new int[9]; //convert 개수는 9개 입니다. 자기 자신을 포함한 9개입니다.
            int conver_count = 0;       //Convert 작업이 이루어진 회수 
            while (options)
            {
                conver_count = 0;
                for (int y = 1; y < ObserverBitmap.Height - 1; y++)
                    for (int x = 1; x < ObserverBitmap.Width - 1; x++)
                    {
                        convert[0] = getlabel[x - 1, y - 1] == 0 ? int.MaxValue : getlabel[x - 1, y - 1];
                        convert[1] = getlabel[x, y - 1] == 0 ? int.MaxValue : getlabel[x, y - 1];
                        convert[2] = getlabel[x + 1, y - 1] == 0 ? int.MaxValue : getlabel[x + 1, y - 1];
                        convert[3] = getlabel[x - 1, y] == 0 ? int.MaxValue : getlabel[x - 1, y];
                        convert[4] = getlabel[x, y] == 0 ? int.MaxValue : getlabel[x, y];
                        convert[5] = getlabel[x + 1, y] == 0 ? int.MaxValue : getlabel[x + 1, y];
                        convert[6] = getlabel[x - 1, y + 1] == 0 ? int.MaxValue : getlabel[x - 1, y + 1];
                        convert[7] = getlabel[x, y + 1] == 0 ? int.MaxValue : getlabel[x, y + 1];
                        convert[8] = getlabel[x + 1, y + 1] == 0 ? int.MaxValue : getlabel[x + 1, y + 1];
                        sort(convert);
                        //0번
                        if (getlabel[x - 1, y - 1] != 0 && getlabel[x - 1, y - 1] != convert[0])
                        {
                            getlabel[x - 1, y - 1] = convert[0];
                            conver_count++;
                        }
                        //1번
                        if (getlabel[x, y - 1] != 0 && getlabel[x, y - 1] != convert[0])
                        {
                            getlabel[x, y - 1] = convert[0];
                            conver_count++;
                        }
                        //2번
                        if (getlabel[x + 1, y - 1] != 0 && getlabel[x + 1, y - 1] != convert[0])
                        {
                            getlabel[x + 1, y - 1] = convert[0];
                            conver_count++;
                        }
                        //3번
                        if (getlabel[x - 1, y] != 0 && getlabel[x - 1, y] != convert[0])
                        {
                            getlabel[x - 1, y] = convert[0];
                            conver_count++;
                        }
                        //4번
                        if (getlabel[x, y] != 0 && getlabel[x, y] != convert[0])
                        {
                            getlabel[x, y] = convert[0];
                            conver_count++;
                        }
                        //5번
                        if (getlabel[x + 1, y] != 0 && getlabel[x + 1, y] != convert[0])
                        {
                            getlabel[x + 1, y] = convert[0];
                            conver_count++;
                        }
                        //6번
                        if (getlabel[x - 1, y + 1] != 0 && getlabel[x - 1, y + 1] != convert[0])
                        {
                            getlabel[x - 1, y + 1] = convert[0];
                            conver_count++;
                        }
                        //7번
                        if (getlabel[x, y + 1] != 0 && getlabel[x, y + 1] != convert[0])
                        {
                            getlabel[x, y + 1] = convert[0];
                            conver_count++;
                        }
                        //8번
                        if (getlabel[x + 1, y + 1] != 0 && getlabel[x + 1, y + 1] != convert[0])
                        {
                            getlabel[x + 1, y + 1] = convert[0];
                            conver_count++;
                        }
                    }
                if (conver_count == 0)
                {
                    options = false;
                }
            }
        }
        private void sort(int[] sortnum)
        {
            int swap;                                                    // 임시변수
            for (int i = 0; i < sortnum.GetLength(0) - 1; i++)
                for (int j = i + 1; j < sortnum.GetLength(0); j++)
                    if (sortnum[i] > sortnum[j])
                    {
                        swap = sortnum[i];
                        sortnum[i] = sortnum[j];
                        sortnum[j] = swap;
                    }
        }
        private void sort(int[] sortnum, int[] sortnum2)
        {
            int swap;                                                    // 임시변수
            for (int i = 0; i < sortnum.GetLength(0) - 1; i++)
                for (int j = i + 1; j < sortnum.GetLength(0); j++)
                    if (sortnum[i] < sortnum[j])
                    {
                        swap = sortnum[i];
                        sortnum[i] = sortnum[j];
                        sortnum[j] = swap;
                        swap = sortnum2[i];
                        sortnum2[i] = sortnum2[j];
                        sortnum2[j] = swap;
                    }
        }
        #endregion
        public void show(int num)
        {
            //Step #6 라벨링 영역중 가장큰 영역과 2번째로 큰 영역
            for (int y = 0; y < label.GetLength(1); y++)
                for (int x = 0; x < label.GetLength(0); x++)
                {
                    try
                    {   //tag가 없을 경우 예외를 처리 해줍니다.
                        if (tag[num] == label[x, y])                 // 가장 큰 영역 색칠
                            ObserverBitmap.SetPixel(x, y, Color.FromArgb(255, 0, 0));
                        else
                        {
                            ObserverBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        }
                    }
                    catch { }//무시하고 처리함.
                }
        }
        public Bitmap GetBitmap()
        {
            return ObserverBitmap;
        }
    }
}
