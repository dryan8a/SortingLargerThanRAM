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
            const int AmountOfRam = 2000000; //not necessarily accurate amount of RAM dummed down so that I can actually see the sort going on multiple cores

            var stream = File.OpenRead("city_temperature.csv");
            int amountOfNeededFiles = (int)(stream.Length / AmountOfRam) + 1;

            var sortingStreams = new FileStream[amountOfNeededFiles];
            for(int i = 0;i<amountOfNeededFiles;i++)
            {
                sortingStreams[i] = File.Create($"file{i}.txt");

                int bytesCount = (int)(i == amountOfNeededFiles - 1 ? stream.Length % AmountOfRam : AmountOfRam); //ensures that the overlap in the last file isn't filled with whitespace
                var bytesToCopy = new byte[bytesCount];
                stream.Read(bytesToCopy, 0, bytesCount);

                sortingStreams[i].Write(bytesToCopy,0,bytesCount);
                sortingStreams[i].Position = 0; //resets position of stream for read
            }
            
            Parallel.For(0, amountOfNeededFiles, i =>
            {
                var bytes = new byte[sortingStreams[i].Length];
                sortingStreams[i].Read(bytes, 0, (int)sortingStreams[i].Length);
                Sorts.MergeSort(bytes);
                sortingStreams[i].Position = 0; //resets position of stream for write so that the file size doesn't double
                sortingStreams[i].Write(bytes, 0, (int)sortingStreams[i].Length);
            });

            var outputStream = File.Create("sortOutput.txt");

            Sorts.KWayMerge(sortingStreams,outputStream);
        }
    }
}
