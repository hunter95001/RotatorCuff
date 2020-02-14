using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace som_cluster
{
    class cluster
    {
        Bitmap clusterBitmap;
        int rec;
        private const int mapsize = 4;
        double[,] map;
        private int inputVeter; //입력 벡터 수
        private const int clusterNode = 3; //출력 노드 수= 클러스터 수
        private double[,] input;
        private const double init_learning_rate  = 0.6; // 초기 학습율
        private const double next_learning_rate = 0.8; // 초기 학습율
        private double Rate = init_learning_rate ;
        private double[,] W;
        private double[,] preD; //경쟁층 노드
        private double[,] pastD; //이전 경쟁층 노드 값
        private int Iterations = 1000;//최대 반복 회수

        public void RUN() {
            int count = 0;
            do //무조건 1번은 실행되어야 하기 때문입니다.
            {
                count++;
            } while (Iterations > count); //반복.

        }

        private void Neighborhood() {

        }


        private void Init() {
            input = new double[clusterBitmap.Width * clusterBitmap.Height,3];
            inputVeter = input.Length;
            map = new double[mapsize, mapsize];
            W = new double[inputVeter, clusterNode];
            int R;
            int G;
            int B;
            for (int x = 0; x < clusterBitmap.Width; x++)
                for (int y = 0; y < clusterBitmap.Height; y++)
                {
                    R = clusterBitmap.GetPixel(x, y).R;
                    G = clusterBitmap.GetPixel(x, y).G;
                    B = clusterBitmap.GetPixel(x, y).B;
                    input[rec,0] = R ;
                    input[rec,1] = G ;
                    input[rec,2] = B ;
                    //Console.WriteLine("R: "+input[rec,0]+ " G: " + input[rec, 1]+ " B: " + input[rec, 2]);
                    rec++;
                }

            Random rand = new Random();
            double random;
            Boolean chake;
            for (int i = 0; i < inputVeter; i++)
                for (int j = 0; j < clusterNode; j++)
                {
                    do
                    {
                      random = Math.Round(rand.NextDouble(), 1);
                    } while (random == 0.0 || random == 1.0);
                    W[i, j] = random;
                    Console.WriteLine(W[i, j]);
                }
            for (int i = 0; i < mapsize; i++)
                for (int j = 0; j < mapsize; j++)
                {
                    do
                    {
                        random = Math.Round(rand.NextDouble(), 1);
                    } while (random == 0.0 || random == 1.0);
                    map[i, j] = random;
                    Console.WriteLine(map[i, j]);
                }



        }

        public cluster(Bitmap bitmap)
        {
            this.clusterBitmap = bitmap;
            Init();
            RUN();

        }

    }
}
