﻿using System;
using System.Collections.Generic;
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
    }
}