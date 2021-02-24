using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace SortingLargerThanRAM
{
    class Program
    {
        static void Main(string[] args)
        {
            const int NumberOfCPUCores = 4; //find using task manager
            const int AmountOfRam = 5000000; //not necessarily accurate amount of RAM dummed down so that I can actually see the sort going on multiple cores

            var stream = File.OpenRead("city_temperature.csv");
            int amountOfNeededFiles = (int)(stream.Length / AmountOfRam) + 1;

            var sortingStreams = new List<FileStream>();
            for(int i = 0;i<amountOfNeededFiles;i++)
            {
                sortingStreams.Add(File.Create($"file{i}.txt"));

                var bytesToCopy = new byte[AmountOfRam];
                stream.Read(bytesToCopy, 0, AmountOfRam);

                int bytesCount = (int)(i == amountOfNeededFiles - 1 ? stream.Length % AmountOfRam : AmountOfRam); //ensures that the overlap in the last file isn't filled with whitespace
                sortingStreams[i].Write(bytesToCopy,0,bytesCount);
                sortingStreams[i].Position = 0; //resets position of stream for read
            }
            
            Parallel.For(0, amountOfNeededFiles, i =>
            {
                var bytes = new byte[AmountOfRam];
                sortingStreams[i].Read(bytes, 0, AmountOfRam);
                Sorts.MergeSort(bytes);
                sortingStreams[i].Position = 0; //resets position of stream for write so that the file size doesn't double
                sortingStreams[i].Write(bytes, 0, AmountOfRam);
            });
        }
    }
}
