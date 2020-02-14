using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace som_cluster
{
        class SOMCluster
        {
            private const int inputVeter = 4; //입력 벡터 수
            private const int clusterNode = 2; //출력 노드 수= 클러스터 수
                                               //입력값 
            private double[,] input = {{ 1, 0, 0, 1 },
                                   { 0, 0, 1, 1 },
                                   { 0, 1, 0, 1 },
                                   { 0, 0, 1, 0 } };

            private const double firstW = 0.6; // 초기 가중치
            private const double LearingW = 0.4; // 다음 가중치
            private double Rate = firstW;
            // private double[,] W; 가중치 값
            private double[,] W ={  { 0.5,0.1},
                                { 0.3,0.7},
                                { 0.6,0.8},
                                { 0.2,0.2}};
            private double[,] preD; //경쟁층 노드
            private double[,] pastD; //이전 경쟁층 노드 값
            private int limitValue = 1000;//최대 반복 회수

            private void Run()
            {
                //W = new double[inputVeter, clusterNode];


                int count = 0;
                while (limitValue > count)
                {
                    preD = new double[inputVeter, clusterNode];
                    for (int k = 0; k < inputVeter; k++)
                    {
                        for (int j = 0; j < clusterNode; j++)
                        {
                            for (int i = 0; i < inputVeter; i++)
                            {
                                preD[k, j] = preD[k, j] + Math.Pow((W[i, j] - input[k, i]), 2);
                            }
                            pastD[k, j] = preD[k, j];
                        }
                        double min = 10;
                        int minValue = 0;
                        for (int i = 0; i < clusterNode; i++)
                        {
                            if (preD[k, i] < min)
                            {
                                min = preD[k, i];
                                minValue = i;
                            }
                        }
                        ChangeW(minValue, k);
                        //show();
                    }
                    count++;
                    LearningRate();
                    Swap();
                    for (int i = 0; i < inputVeter; i++, Console.WriteLine())
                        for (int j = 0; j < clusterNode; j++)
                        {
                            Console.Write("\t" + pastD[i, j] + " \t" + preD[i, j]);
                        }
                }
            }
            private void Swap()
            {
                double[,] swap = new double[inputVeter, inputVeter];
                for (int i = 0; i < inputVeter; i++)
                    for (int j = 0; j < inputVeter; j++)
                    {
                        swap[j % inputVeter, i] = input[(inputVeter - 1 + j) % inputVeter, i];
                    }
                input = swap;
            }
            //학습율 조절
            private void LearningRate()
            {
                Rate = Rate * LearingW;
            }
            //가중치 조정
            private void ChangeW(int Number, int count)
            {
                for (int i = 0; i < inputVeter; i++, Console.WriteLine())
                {
                    W[i, Number] = W[i, Number] + Rate * (input[count, i] - W[i, Number]);
                }
            }
            //출력
            private void show()
            {

                Console.WriteLine("***********가중치 확인***************");
                for (int i = 0; i < inputVeter; i++, Console.WriteLine())
                    for (int j = 0; j < clusterNode; j++)
                    {
                        Console.Write(W[i, j] + " \t");
                    }
                Console.WriteLine("**************************************\n");

                Console.WriteLine("***********경쟁층 노드 확인***************");
                for (int i = 0; i < clusterNode; i++, Console.WriteLine())
                {
                    //Console.Write(preD[i] + " \t");
                }
                Console.WriteLine("**************************************\n");

            }
            //초기화
            private void Init()
            {

                pastD = new double[inputVeter, clusterNode];
            }
            public SOMCluster()
            {
                Init();
                Run();
            }

        }
    }
/*참고 자료
http://twinw.tistory.com/15
*/