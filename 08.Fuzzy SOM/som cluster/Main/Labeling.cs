using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace som_cluster.Main
{
    class Labeling
    {
        Bitmap ObserverBitmap;
        int[,] label;       //라벨링 번호가 담겨져 있는 배열
        int[,] color;       // 컬러 색상이 담겨져 있는 배열
        int[] tag;          // 색상 번호가 담겨져 있는 번호
        int count = 0;
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
    
        public void AIRun()
        {
            Navigation();   //8방향 윤곽선 탐색 기법및 번호 부여한다.
            AITagging();    //인접한 도형의 번호로 바꾼다.
            show(3);
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

        private void Detection8(int x, int y)
        {
            Boolean option = true;  //while문 종료 조건.
            count++;                //라벨 번호를 매기기 위해서

           
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
    
        private void AITagging()
        {
            int[] countarray = new int[count + 1]; 
            tag = new int[count + 1];               

            for (int i = 0; i < countarray.Length; i++)
            {
                tag[i] = i;                         
            }
            countarray.Initialize();               
            Convert(label);                        
            for (int y = 0; y < label.GetLength(1); y++)
                for (int x = 0; x < label.GetLength(0); x++)
                {
                    countarray[label[x, y]]++;      
                }
            countarray[0] = 0;                      
            sort(countarray, tag);        
        }

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