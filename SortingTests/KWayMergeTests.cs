using SortingLargerThanRAM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace SortingTests
{
    public class KWayMergeTests
    {
        [Theory]
        [ClassData(typeof(KWayMergeData))]
        public void KWayMergeTest(byte[] expected, byte[][] input,char filePostfix)
        {
            var actualStream = File.Create($"UnitTestOutput{filePostfix}.txt");

            var inputStreams = new FileStream[input.Length];
            for(int i = 0;i<input.Length;i++)
            {
                inputStreams[i] = File.Create($"UnitTestInput{filePostfix}{i}.txt");
                inputStreams[i].Write(input[i]);
                inputStreams[i].Position = 0;
            }

            Sorts.KWayMerge(inputStreams, actualStream);

            actualStream.Position = 0;
            byte[] actual = new byte[actualStream.Length];
            actualStream.Read(actual, 0, (int)actualStream.Length);

            Assert.Equal(actual.Length, expected.Length);
            for(int i = 0;i<actual.Length;i++)
            {
                Assert.Equal(actual[i], expected[i]);
            }
        }
    }

    public class KWayMergeData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new byte[]{2,3,4,5,6,7,8,9,10,16,20,21 }, new[] { new byte[] {2,7,16 },new byte[] {5,10,20 },new byte[] {3,6,21 },new byte[] { 4, 8, 9 } }, 'A'};
            yield return new object[] { new byte[] {1,1,1,1,2,2,2,2,3,3,3,3 }, new[] { new byte[] {1,1,1,3 },new byte[] { 1, 2, 2, 3 },new byte[] { 2, 2, 3, 3 } }, 'B' };
            yield return new object[] { new byte[] { 1 }, new[] { new byte[] { 1 } }, 'C' };
            yield return new object[] { new byte[] { 1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8 }, new[] { new byte[] { 1, 7 }, new byte[] { 5, 6 }, new byte[] { 2, 2 }, new byte[] { 1, 3 }, new byte[] { 3, 4, 8 } }, 'D' };
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
