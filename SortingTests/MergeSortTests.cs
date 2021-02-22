using SortingLargerThanRAM;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace SortingTests
{
    public class MergeSortTests
    {
        [Theory] 
        [ClassData(typeof(MergeSortData))]
        public void MergeSortTest(int[] expectedSorted, int[] unsorted)
        {
            Assert.Equal(expectedSorted.Length, unsorted.Length);

            Sorts.MergeSort(unsorted);

            for(int i = 0;i<expectedSorted.Length;i++)
            {
                Assert.Equal(expectedSorted[i], unsorted[i]);
            }
        }
    }

    public class MergeSortData : IEnumerable<object[]>
    { 
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {new int[]{1,2,3,4,5}, new int[] {5,3,2,4,1}};
            yield return new object[] { new int[] { int.MinValue, 0, int.MaxValue }, new int[] { 0, int.MaxValue, int.MinValue } };
            yield return new object[] { new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 6, 3, 9, 1, 4, 8, 0, 2, 7, 5 } };
        }

        IEnumerator<object[]> IEnumerable<object[]>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
