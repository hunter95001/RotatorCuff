using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace som_cluster.Processing.AI.DataStruct
{
    class BubbleSort
    {


        double[,] sortnum;
        public BubbleSort(double[,] getnum) {
            Console.WriteLine(getnum.GetLength(0)+" "+getnum.GetLength(1));
            sortnum = getnum;
            double[] swaparray= new double[getnum.GetLength(1)];

            for (int i = 0; i < getnum.GetLength(0); i++)
            {
                for (int j = 0; j < getnum.GetLength(1); j++)
                {
                    swaparray[j] = getnum[i, j];
                }
                swaparray =run(swaparray);

                for (int j = 0; j < getnum.GetLength(1); j++)
                {
                    sortnum[i, j] = swaparray[j];
                }
            }
        }

        public double[,] getDouble()
        {
            return sortnum;
        }

        double[] sort;


        public BubbleSort(double[] getnum)
        {
            sort=run(getnum);
        }
        private double[] run(double[] getnum) {
            double swap;       
            for (int i = 0; i < getnum.GetLength(0) - 1; i++)
                for (int j = i + 1; j < getnum.GetLength(0); j++)
                    if (getnum[i] > getnum[j])
                    {
                        swap = getnum[i];
                        getnum[i] = getnum[j];
                        getnum[j] = swap;
                    }
         
            return getnum;
        }
        public double[] getdouble()
        {
            return sort;
        }
        int[] sortint;

        public BubbleSort(int[] getnum)
        {
            sortint = run(getnum);
        }
        private int[] run(int[] getnum)
        {
            int swap;
            for (int i = 0; i < getnum.GetLength(0) - 1; i++)
                for (int j = i + 1; j < getnum.GetLength(0); j++)
                    if (getnum[i] > getnum[j])
                    {
                        swap = getnum[i];
                        getnum[i] = getnum[j];
                        getnum[j] = swap;
                    }

            return getnum;
        }
        public int[] getint()
        {
            return sortint;
        }


    }
}
