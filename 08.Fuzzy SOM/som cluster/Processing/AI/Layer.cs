using som_cluster.Processing.AI.DataStruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace som_cluster.Processing.AI
{
    class Layer
    {
        private const int MAX = 256;
        private int mapsize;              // 맵의 사이즈 5*5
        private const int ephocs = 100;
        private double Lrate = 0.6;
        private double Momentem = 0.4;              //1-Larate;
        private const double Ecrit = 0.05;


        private int[] winnernode;
        private double[] input;                      // 컬러값 받는 변수
        private double[] W;                         // 가중치 
        private double[] CenterFuzzy;
        private double[,] map;                      // 맵

        private int Radius;
        private Boolean Continue = true;            // 종료 조건
        private String FirstWeight;

        public Layer(double[] getinput, int mapsize)
        {
            Console.WriteLine("2층 실행");
            this.mapsize = mapsize;
            this.input = getinput;
            Init();
        }

        public void run()
        {
            Radius = 1;
            Procesing();
        }

        #region 초기화
        private void Init()
        {
            #region Step #2 가중치 초기화
            map = new double[mapsize, mapsize];   // 맵 크기 3*3
            W = new double[mapsize * mapsize];
            Radius = (mapsize + 1) / 2;
            Random random = new Random();
            for (int x = 0; x < W.GetLength(0); x++)
            {
                W[x] = random.NextDouble();
                W[x] = Math.Round(W[x], 4);
            }
            BubbleSort bubbleSort = new BubbleSort(W);
            W = bubbleSort.getdouble();

            for (int x = 0; x < W.GetLength(0); x++)
            {
                FirstWeight += W[x].ToString();
                if (x != W.GetLength(0) - 1)
                    FirstWeight += " , ";
            }
            FirstWeight += "\r\n========\r\n";

            #endregion

        }
        #endregion

        private void Procesing()
        {
            Console.WriteLine("2층 초기 가중치 ");
            for (int x = 0; x < W.Length; x++)
            {
                Console.WriteLine(W[x]);
            }

            int Timecount = 0;                                              // 반복 회수 초기화
            do
            {
                Distance_Calc();                                            // 거리값 계산
                Wight_Change();
                Rate_Change();
                ComputeTSS(Timecount);
                Timecount++;                                                // 반복회수  
            } while (Continue);                                             // true = 실행 , false = 종료
        }
        #region 거리값 계산
        /*
            입력벡터와 가중치사이의 거리값을 통해서 가장 짧은 노드를 승자노드로 할당해줍니다
            거리값 = 유클리드 거리공식 사용
        */
        private void Distance_Calc()
        {
            double Min = 999999;
            winnernode = new int[input.GetLength(0)];
            double[] Distance = new double[W.GetLength(0)];
            for (int x = 0; x < input.GetLength(0); x++)
                for (int z = 0; z < W.GetLength(0); z++)
                {
                    Min = 999999;
                    for (int i = 0; i < Distance.Length; i++)
                    {
                        Distance[i] = 0;
                        Distance[i] = Math.Sqrt(Distance[i] + Math.Pow(input[x] - W[i], 2));
                    }
                    for (int i = 0; i < Distance.Length; i++)
                        if (Min > Distance[i])
                        {
                            Min = Distance[i];
                            winnernode[x] = i;
                        }
                }
        }
        #endregion

        #region 가중치 조정
        private void Wight_Change()
        {
            double[] sum = new double[mapsize * mapsize];
            int[] sumcount = new int[mapsize * mapsize];
            CenterFuzzy = new double[sum.Length];
            int inside = 0;
            sum.Initialize();
            sumcount.Initialize();

            /* 1차원인 W값을 2차원으로 바꿈으로써 반경 계산을 편하게 한다*/
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = W[inside];
                    inside++;
                }
            for (int x = 0; x < winnernode.Length; x++)
            {
                sum[winnernode[x]] += input[x];
                sumcount[winnernode[x]]++;
            }

            for (int x = 0; x < sumcount.Length; x++)
            {
                if (x == 0 && sumcount[x] != 0)
                {
                    CenterFuzzy[0] = 0;
                }
                else
                {
                    CenterFuzzy[x] = Math.Round(sum[x] / sumcount[x], 3);
                }
                Console.WriteLine("중심값 " + CenterFuzzy[x] + " 분자" + sum[x] + " 분모 " + sumcount[x]);
            }

            /* 이웃 노드가중치 변경. */
            for (int z = 0; z < CenterFuzzy.Length; z++)
            {
                int row = winnernode[z] / map.GetLength(0);   // 기수 가로 -tuple
                int col = winnernode[z] % map.GetLength(1); // 차수 세로-degree
                int rowx = 0;
                int coly = 0;
                //Console.WriteLine("승자노드 "+winnernode[z]+" "+ row+" "+col);

                for (int x = (Radius - 1) * -1; x < Radius; x++)
                    for (int y = (Radius - 1) * -1; y < Radius; y++)
                    {
                        if ((row + x) <= -1)
                        {
                            rowx = 0;
                        }
                        else if ((row + x) >= map.GetLength(0))
                        {
                            rowx = map.GetLength(0);
                        }
                        else if ((col + y) <= -1)
                        {
                            coly = 0;
                        }
                        else if ((col + y) >= map.GetLength(1))
                        {
                            coly = map.GetLength(0);
                        }
                        else
                        {
                            rowx = row + x;
                            coly = col + y;
                            if (double.IsNaN(CenterFuzzy[z]) == false)
                            {
                                map[rowx, coly] = Math.Round(map[rowx, coly] + Lrate * (CenterFuzzy[z] - map[rowx, coly]), 5);
                            }
                        }
                    }
            }

            /* 2차원인 map을 다시 1차원으로 바꿔 W에 저장함. */
            inside = 0;
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    W[inside] = Math.Round(map[x, y], 5);

                    Console.WriteLine("가중치 " + W[inside] + " " + map[x, y]);
                    inside++;
                }
        }
        #endregion
        #region 학습률 조정
        private void Rate_Change()
        {
            Lrate = (1.0 - Momentem);
            if (Momentem > 0.9)
            {
                Radius = 1;
            }
            else if (Momentem > 0.5)
            {
                Radius = 2;
            }
            else
            {
                Radius = 3;
            }
            Console.WriteLine("학습률 " + Lrate + " 모멘텀 " + Momentem + " 반경 " + Radius);
        }
        #endregion

        #region TSS값 계산
        private void ComputeTSS(int Timecount)
        {
            double pss = 0;
            double tss = 0;
            double error = 0;
            int correct = 0, incorrect = 0;

            Console.WriteLine(CenterFuzzy.Length + " " + W.Length + " " + winnernode.Length);
            for (int x = 0; x < CenterFuzzy.Length; x++)
            {
                if (CenterFuzzy[x].Equals(double.NaN) || true && double.IsNaN(W[x])) //개수가 0이면 0/0 => NaN오류가 발생한다.
                {
                    correct++;
                }
                else
                {
                    error = CenterFuzzy[x] - W[x];
                    pss = error * error;
                    tss += pss;
                    if (error < 0.1)
                        correct++;
                }
                Console.WriteLine(x + " 번째 " + CenterFuzzy[x] + " " + W[x]);
            }

            incorrect = CenterFuzzy.Length;
            incorrect = incorrect - correct;

            if (incorrect != 0)
                Momentem = (double)correct / input.Length;
            tss = tss / 2;
            Console.WriteLine("반복회수 " + Timecount + " TSS " + tss + " incorrect " + incorrect + " correct " + correct + " Momentem " + Momentem);
            if (Timecount > ephocs || Ecrit > tss || incorrect == 0)
                Continue = false;
        }

        #endregion


        public int[] getwinner()
        {
            return winnernode;
        }

        public String getString()
        {

            return FirstWeight;
        }

        public void setWeight(double[] getweight)
        {
            W = getweight;
            this.mapsize = (int)Math.Sqrt(W.Length);
        }
    }
}