using System;
using System.Threading.Tasks;
using System.IO;

namespace SortingLargerThanRAM
{
    class Program
    {
        static void Main(string[] args)
        {
            const int NumberOfCPUCores = 4; //find using task manager
            const int AmountOfRam = 2000000; //not necessarily accurate amount of RAM dummed down so that I can actually see the sort going on multiple cores

            var stream = File.OpenRead("2019-Oct.csv");
            long length = stream.Length;
            int amountOfNeededFiles = (int)(length / AmountOfRam) + 1;

            Parallel.For(0, amountOfNeededFiles, i =>
            {
                //byte[] bytes = new byte[AmountOfRam]; 
                //stream.Read(bytes, 0, AmountOfRam);
                //Sorts.MergeSort(bytes);
            });
        }

       
    }
}
