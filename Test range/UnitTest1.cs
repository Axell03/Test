using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test_range
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestContains()
        {
            Range range = new Range(2, 6);
            Assert.IsTrue(range.Contains(4)); // 4 debería estar dentro del rango [2,6)
            Assert.IsFalse(range.Contains(10)); // 10 no debería estar dentro del rango [2,6)
        }

        [TestMethod]
        public void TestGetAllPoints()
        {
            Range range = new Range(2, 6);
            CollectionAssert.AreEquivalent(new[] { 2, 3, 4, 5 }, range.GetAllPoints().ToArray()); // Se espera que todos los puntos dentro del rango sean 2, 3, 4 y 5
        }

        [TestMethod]
        public void TestContainsRange()
        {
            Range range1 = new Range(2, 5);
            Range range2 = new Range(3, 5);
            Range range3 = new Range(7, 10);

            Assert.IsFalse(range1.ContainsRange(range3));
            Assert.IsTrue(range1.ContainsRange(range2));
        }
    }

    public class Range
    {
        public int Start { get; }
        public int End { get; }

        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(int num)
        {
            return Start <= num && num < End;
        }

        public HashSet<int> GetAllPoints()
        {
            HashSet<int> points = new HashSet<int>();
            for (int i = Start; i < End; i++)
            {
                points.Add(i);
            }
            return points;
        }

        public bool ContainsRange(Range other)
        {
            return Start <= other.Start && End >= other.End;
        }
    }

}

