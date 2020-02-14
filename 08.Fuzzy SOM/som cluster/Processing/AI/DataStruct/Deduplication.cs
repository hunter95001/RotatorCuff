using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace som_cluster.Processing.AI.DataStruct
{
    class Deduplication
    {
        int[] debple_int;  //queue의 사이즈 만큼 배열을 생성해줍니다.
        int size_qu; //queue에 들어가있는 크기가 중복을 제거한 배열의 크기
        public Deduplication(int[] sortarr) {
            int[] swap = new int[sortarr.Length];
            for (int x = 0; x < sortarr.Length; x++)
            {
                swap[sortarr[x]] = 1;
            }
            //중복 제거
            Queue<int> dubple = new Queue<int>();
            for (int i = 0; i < swap.Length; i++)
            {
                //색상이 있으면 1 아니면 0으로 표시했기 때문에 색상은 큐에 넣어주는 역할을 합니다.
                if (swap[i] != 0)
                    dubple.Enqueue(i);
            }
            size_qu = dubple.Count; 
            debple_int = new int[size_qu]; 
            //size_qu -> dubple.Count를 사용하면 Dequeue하게되면 논리적인 에러가 발생하죠?
            for (int i = 0; i < size_qu; i++)
            {
                debple_int[i] = dubple.Dequeue();
            }
            
        }

        public int[] GetInt() {
            return debple_int;
        }

    }
}
