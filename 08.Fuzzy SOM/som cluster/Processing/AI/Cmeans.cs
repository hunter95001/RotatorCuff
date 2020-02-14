using som_cluster.Processing.AI.DataStruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace som_cluster.Processing
{
    class Cmeans
    {
        //Step #1 클러스터 개수를 정해줍니다
        const int clusterNum = 9;                 //클러스터 개수
        private int MAX = 0;
        int[] centerNum = new int[clusterNum];    //중심 좌표
        int[] inputCount = new int[256];          //256가지 색상의 개수 [EX 234번의 색깔은 4365개가 있다]
        int[,] sumCount = new int[clusterNum, 2]; //중심좌표를 구하기 위한 2차원 배열
                                                  //0 = 분모 (색상의 개수)
                                                  //1 = 분자 (색상 * 색상의 개수)
        int option = 0;                           //반복문 종료
        int[] Input;
        int[] output;


        public Cmeans(int[] getinput)
        {
            Input = new int[getinput.Length];
            output = new int[getinput.Length];

            for (int x = 0; x < getinput.Length; x++)
            {
                Input[x] = getinput[x];
                //Console.WriteLine("입력값 "+Input[x]);
                MAX = Math.Max(MAX,Input[x]);
            }
            inputCount = new int[MAX+1];

        }

        public void Run() {
            processing();

            //Pallete_set();
            //Select(0); //<-보고싶은 부분만 선택하는 함수
        }
        private void processing()
        {
            //Step #2 클러스터의 중심 좌표를 정적으로 할당 해줍니다. [255는 색상의 최대값]
            for (int i = 0; i < clusterNum; i++)
            {
                int h = MAX / clusterNum;
                centerNum[i] = h + h * i;
            }

            //컬러 색상의 개수를 구함
            for (int x = 0; x < Input.Length; x++)
            {
                inputCount[Input[x]]++;
            }


            do
            {
                //Step #3 클러스터 범위 기준으로 중심좌표를 설정합니다 [중심좌표 =시그마(색상*개수)/시그마(개수)]
                for (int i = 0; i < inputCount.Length; i++) //256은 색상의 최대값 [0~255]
                {
                    int sum = i * inputCount[i]; //개수랑 색상의 곱
                    for (int j = 0; j < clusterNum; j++)
                    {
                        int startPoint = j == 0 ? 0 : (centerNum[j - 1] + centerNum[j]) / 2;//0 보다 작으면 0
                        int endPoint = j == clusterNum - 1 ? 255 : (centerNum[j] + centerNum[j + 1]) / 2;//클러스터개수 보다 크면 255
                        if (startPoint <= i && i <= endPoint)
                        {
                            sumCount[j, 0] = sumCount[j, 0] + inputCount[i];
                            sumCount[j, 1] = sumCount[j, 1] + sum;
                        }
                    }
                }

                // Step #4 중심좌표가 이전 좌표와 같은지 확인합니다
                //다르면 3번으로 가서 반복합니다.
                for (int i = 0; i < clusterNum; i++)
                {
                    sumCount[i, 0] = sumCount[i, 0] == 0 ? 1 : sumCount[i, 0];
                    if (centerNum[i] == sumCount[i, 1] / sumCount[i, 0])
                    {
                        option++;
                    }
                    else
                    {
                        option = 0;
                        centerNum[i] = sumCount[i, 1] / sumCount[i, 0];
                    }
                }
            } while (option <= clusterNum);
          

            for (int i = 0; i < Input.Length; i++)
                for (int j = 0; j < clusterNum; j++)
                {
                    int startPoint = j == 0 ? 0 : (centerNum[j - 1] + centerNum[j]) / 2;//0 보다 작으면 0
                    int endPoint = j == clusterNum - 1 ? MAX : (centerNum[j] + centerNum[j + 1]) / 2;//클러스터개수 보다 크면 255
                    if (startPoint <= Input[i] && Input[i] <= endPoint)
                    {
                        output[i] = j;
                    }
                   
                }

            for (int x = 0; x < output.Length; x++)
            {
                output[x] = output[x] - output[0];
                //Console.WriteLine("output" + output[x]);
            }
        }

        public int[] getOutput()
        {
            return output;
        }

    }
}
