using som_cluster.Processing.AI.DataStruct;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace som_cluster.Processing.AI
{
    class Fuzzsom
    {
        //Step #1 클러스터 개수를 정해줍니다
        Layer layer;
        Bitmap obserbitmap;                         // SOM 적용된 Bitmap
        Bitmap Palletbitmap;                        // 양자화 Bitmap
        Bitmap LayerPalletbitmap;                        // 양자화 Bitmap

        private const int MAX = 256;
        private const int mapsize = 7;              // 맵의 사이즈 5*5
        private const int ephocs = 100;
        private const double Ecrit = 0.05;

        private double Lrate = 0.6;
        private double Momentem = 0.4;              //1-Larate;

        private int[] winnernode;
        private int[] Layerwinnernode;
        private int[] countcolor = new int[MAX];
        private int[,] color;                      // 컬러값 받는 변수

        private double[] W;                         // 가중치 
        private double[] fuzzyinput;
        private double[] CenterFuzzy;
        private double[] nextinput;
        private double[,] map;                      // 맵

        private int Radius = 1;
        private int nextMapsize;

        private String FirstWeight;
        private Boolean Continue = true;            // 종료 조건

        public Fuzzsom(Bitmap bitmap)
        {
            Console.WriteLine("SOM 시작");
            obserbitmap = new Bitmap(bitmap);
            Init();
        }

        public void Layer2()
        {
            int mapcount = 0;
            for (int x = 0; x < CenterFuzzy.Length; x++)
            {
                if (CenterFuzzy[x].Equals(double.NaN) == true && double.IsNaN(CenterFuzzy[x]))
                {
                    //NaN이 발생하는 이유 -> 승자노드로 선택받지 못했기 때문이다.
                }
                else
                {
                    mapcount++;
                }
            }

            nextMapsize = (int)Math.Floor(Math.Sqrt(mapcount));   //내림하는 이유 =>  
            nextinput = CenterFuzzy;
            layer = new Layer(nextinput, nextMapsize);
            Console.WriteLine("다음 맵의 크기" + nextMapsize);
        }

        #region Run
        public void Run()
        {
            Console.WriteLine("초기 가중치");
            for (int i = 0; i < W.Length; i++)
            {
                Console.WriteLine(W[i]);
            }

            int Timecount = 0;                                              // 반복 회수 초기화
            do
            {
                Distance_Calc();                                            // 거리값 계산
                Wight_Change();                                             // 가중치 계산
                Rate_Change();                                              // 학습률 조절
                ComputeTSS(Timecount);                                      // TSS값 계산
                Timecount++;                                                // 반복회수  
            } while (Continue);                                             // true = 실행 , false = 종료
        }

        public void Layer_Run()
        {

            layer.run();
            int[] winner = layer.getwinner();
            Layerwinnernode = new int[winnernode.Length];
            //foreach (var item in winner)
            //{
            //   /Console.WriteLine("위너! " + item);
            //}
            for (int x = 0; x < winnernode.Length; x++)
            {
                Layerwinnernode[x] = winner[winnernode[x]];
                //Console.WriteLine(Layerwinnernode[x]);
            }
            //Console.WriteLine(winner.Length + " " + winnernode.Length);
        }
        #endregion
        #region Init[초기화]
        /*
         Step #1 퍼지 소속 함수 구함

         Step #2 가중치 초기화 
         0 ~1 사이의 값을 가집니다.
         가중치 개수 = [INPUT, MapSIZE ] 256,9

          2 Layer층 초기화
          Step #1 다음층의 맵의 크기를 정함
        */
        private void Init()
        {
            #region Step #1 퍼지 소속함수
            Fuzzy fuzzy = new Fuzzy();
            double max = 0, min = MAX;
            color = new int[obserbitmap.Width, obserbitmap.Height];
            fuzzyinput = new double[MAX];
            countcolor.Initialize();

            for (int x = 0; x < color.GetLength(0); x++)
                for (int y = 0; y < color.GetLength(1); y++)
                {
                    color[x, y] = obserbitmap.GetPixel(x, y).R;
                    countcolor[color[x, y]]++;
                    max = Math.Max(color[x, y], max);
                    min = Math.Min(color[x, y], min);
                }
            for (int x = 0; x < MAX; x++)
            {
                fuzzyinput[x] = fuzzy.MemberShip(max, min, x);
            }
            #endregion

            #region Step #2 가중치 초기화
            map = new double[mapsize, mapsize];   // 맵 크기 3*3

            W = new double[mapsize * mapsize];
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
        public void Layer_Init(double[] layerweight)
        {
            layer.setWeight(layerweight);
        }
        #endregion
        #region 거리값 계산
        /*
            입력벡터와 가중치사이의 거리값을 통해서 가장 짧은 노드를 승자노드로 할당해줍니다
            거리값 = 유클리드 거리공식 사용
         */

        private void Distance_Calc()
        {
            double Min = 999999;
            winnernode = new int[fuzzyinput.GetLength(0)];
            double[] Distance = new double[W.GetLength(0)];

            for (int x = 0; x < fuzzyinput.GetLength(0); x++)
                for (int z = 0; z < W.GetLength(0); z++)
                {
                    Min = 999999;
                    for (int i = 0; i < Distance.Length; i++)
                    {
                        Distance[i] = 0;
                        Distance[i] = Math.Sqrt(Distance[i] + Math.Pow(fuzzyinput[x] - W[i], 2));
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
                sum[winnernode[x]] += countcolor[x] * fuzzyinput[x];
                sumcount[winnernode[x]] += countcolor[x];
            }

            for (int x = 0; x < sumcount.Length; x++)
            {
                
                CenterFuzzy[x] = Math.Round(sum[x] / sumcount[x], 3);
                Console.WriteLine("중심값 " + CenterFuzzy[x]+" 분자"+ sum[x]+" 분모 "+sumcount[x] );
            }

            for (int z = 0; z < CenterFuzzy.Length-1; z++)
            {
                int row = winnernode[z] / map.GetLength(0);   // 기수 가로 -tuple
                int col = winnernode[z] % map.GetLength(1); // 차수 세로-degree
                int rowx = 0;
                int coly = 0;
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
                    W[inside] = Math.Round(map[x, y],5);

                    Console.WriteLine(W[inside] + " ");
                    inside++;
                }
        }
        #endregion
        #region 학습률 조절
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
                Radius = (map.GetLength(0) + 1) / 2;
            }
            Console.WriteLine("학습률 " + Lrate + " 모멘텀 " + Momentem + " 반경 " + Radius);
        }
        #endregion
        #region TSS값 계산
        private void ComputeTSS(int Timecount) {
            double pss = 0;
            double tss = 0;
            double error = 0;
            int correct = 0, incorrect = 0;

            Console.WriteLine(CenterFuzzy.Length + " " + W.Length + " " + winnernode.Length);
            for (int x = 0; x < CenterFuzzy.Length; x++)
            {
                //NaN이 발생하는 이유 -> 승자노드로 선택받지 못했기 때문이다.
                if (CenterFuzzy[x].Equals(double.NaN) || true && double.IsNaN(W[x])) 
                {
                    correct++;  //승자노드가 아니기 때문에 같다고 지정한다.
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
                Momentem = (double)correct / (color.GetLength(0) * color.GetLength(1));
            tss = tss / 2;
            Console.WriteLine("반복회수 " + Timecount + " TSS " + tss + " incorrect " + incorrect + " correct " + correct + " Momentem " + Momentem);
            if (Timecount > ephocs || Ecrit > tss || incorrect == 0)
                Continue = false;
        }

        #endregion
        #region Clustering
        int[] cluster;
        private void Clustering(int[] getwinner)
        {
            int[] getint;
            Cmeans cmeans = new Cmeans(getwinner);
            cmeans.Run();
            getint = cmeans.getOutput();
            //for (int x = 0; x < getint.Length; x++)
            //{
            //    Console.WriteLine("클러스터" + getint[x]);
            //}
            cluster = getint;
        }

        private Bitmap Pallet()
        {
            Palletbitmap = new Bitmap(obserbitmap);
            int[] Pallet_color = new int[3];

            for (int x = 0; x < obserbitmap.Width; x++)
                for (int y = 0; y < obserbitmap.Height; y++)
                {
                    int num = cluster[color[x, y]];
                    switch (num)
                    {
                        case 0:
                            Pallet_color[0] = 255;
                            Pallet_color[1] = 0;
                            Pallet_color[2] = 0;
                            break;
                        case 1:
                            Pallet_color[0] = 16;
                            Pallet_color[1] = 247;
                            Pallet_color[2] = 243;
                            break;
                        case 2:
                            Pallet_color[0] = 14;
                            Pallet_color[1] = 222;
                            Pallet_color[2] = 245;
                            break;
                        case 3:
                            Pallet_color[0] = 134;
                            Pallet_color[1] = 220;
                            Pallet_color[2] = 39;
                            break;
                        case 4:
                            Pallet_color[0] = 227;
                            Pallet_color[1] = 223;
                            Pallet_color[2] = 31;
                            break;
                        case 5:
                            Pallet_color[0] = 247;
                            Pallet_color[1] = 146;
                            Pallet_color[2] = 21;
                            break;
                        case 6:
                            Pallet_color[0] = 255;
                            Pallet_color[1] = 0;
                            Pallet_color[2] = 223;
                            break;
                        case 7:
                            Pallet_color[0] = 207;
                            Pallet_color[1] = 104;
                            Pallet_color[2] = 48;
                            break;
                        case 8:
                            Pallet_color[0] = 0;
                            Pallet_color[1] = 0;
                            Pallet_color[2] = 0;
                            break;
                    }
                    Palletbitmap.SetPixel(x, y, Color.FromArgb(Pallet_color[0], Pallet_color[1], Pallet_color[2]));
                }
            return Palletbitmap;
        }
        #endregion
        #region GET ,SET
        public void setWeight(double[] getweight)
        {
            W = getweight;
        }

        public String getWeight()
        {
            return FirstWeight;
        }

        public string getLayerweight() {
            return layer.getString();
        }

        public Bitmap getBitmap()
        {
            Clustering(winnernode);
            Palletbitmap = new Bitmap(Pallet());
            return Palletbitmap;
        }

        public Bitmap getLayer_Bitmap()
        {
            Clustering(Layerwinnernode);
            LayerPalletbitmap = Pallet();
            return LayerPalletbitmap;
        }

        #endregion

    }
}
