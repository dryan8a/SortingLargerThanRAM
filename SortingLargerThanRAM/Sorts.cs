using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SortingLargerThanRAM
{
    public static class Sorts
    {
        public static void MergeSort<T>(T[] vals) where T : IComparable<T>
        {
            if (vals.Length <= 1) return;

            int leftLength = vals.Length / 2;

            T[] left = new T[leftLength];
            T[] right = new T[vals.Length - leftLength];

            for (int i = 0; i < leftLength; i++)
            {
                left[i] = vals[i];
            }
            for (int i = 0; i < vals.Length - leftLength; i++)
            {
                right[i] = vals[i + leftLength];
            }

            MergeSort(left);
            MergeSort(right);

            Merge(vals, left, right);
        }

        static void Merge<T>(T[] output, T[] left, T[] right) where T : IComparable<T>
        {
            int currentLeft = 0;
            int currentRight = 0;
            for (int i = 0; i < output.Length; i++)
            {
                if (currentLeft < left.Length && (currentRight == right.Length || left[currentLeft].CompareTo(right[currentRight]) < 0))
                {
                    output[i] = left[currentLeft];
                    currentLeft++;
                }
                else
                {
                    output[i] = right[currentRight];
                    currentRight++;
                }
            }
        }

        public static void KWayMerge(FileStream[] filesToMerge, FileStream output)
        {
            var originalBytes = new Queue<LoserTree<byte>.Node<byte>>(); //gets the lowest byte from each file
            for(int i = 0;i<filesToMerge.Length;i++)
            {
                filesToMerge[i].Position = 0;
                var byteToRead = new byte[1];
                filesToMerge[i].Read(byteToRead, 0, 1);
                originalBytes.Enqueue(new LoserTree<byte>.Node<byte>(byteToRead[0], i));
            }

            var tree = new LoserTree<byte>(originalBytes);

            while(!tree.Winner.IsEmpty)
            {
                output.Write(new[] { tree.Winner.Value }, 0, 1);

                bool streamIsEmpty = filesToMerge[tree.Winner.OriginIndex].Position == filesToMerge[tree.Winner.OriginIndex].Length;

                var byteToRead = new byte[1];
                filesToMerge[tree.Winner.OriginIndex].Read(byteToRead, 0, 1);
                
                if(filesToMerge[3].Length == filesToMerge[3].Position)
                {
                    //var bytesToRead = new byte[3];
                    //filesToMerge[0].Read(bytesToRead, 0, 1);
                    //filesToMerge[1].Read(bytesToRead, 1, 1);
                    //filesToMerge[2].Read(bytesToRead, 2, 1);
                    ;
                }

                if(byteToRead[0] != tree.Winner.Value)
                {
                    ;
                }

                var newNode = new LoserTree<byte>.Node<byte>(byteToRead[0], tree.Winner.OriginIndex, streamIsEmpty);
                tree.RefreshTree(tree.Winner, newNode);
            }
            ;
        }
    }
}
