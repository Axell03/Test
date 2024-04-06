using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Testrange
{
    [TestFixture]
    public class RangeTests
    {
        [Test]
        public void TestContains()
        {
            Range range = new Range(2, 6);
            Assert.IsTrue(range.Contains(4));
            Assert.IsTrue(range.Contains(2));
            Assert.IsFalse(range.Contains(-1));
            Assert.IsFalse(range.Contains(1));
            Assert.IsFalse(range.Contains(6));
            Assert.IsFalse(range.Contains(10));
        }

        [Test]
        public void TestGetAllPoints()
        {
            Range range = new Range(2, 6);
            var allPoints = range.GetAllPoints();
            Assert.AreEqual(new HashSet<int> { 2, 3, 4, 5 }, allPoints);
        }

        [Test]
        public void TestContainsRange()
        {
            Range range1 = new Range(2, 5);
            Range range2 = new Range(3, 5);
            Range range3 = new Range(7, 10);

            Assert.IsFalse(range1.ContainsRange(range3));
            Assert.IsFalse(range1.ContainsRange(range2));
            Assert.IsTrue(new Range(2, 10).ContainsRange(new Range(3, 5)));
            Assert.IsTrue(new Range(3, 5).ContainsRange(new Range(3, 5)));
        }

        [Test]
        public void TestEndPoints()
        {
            Assert.AreEqual(new HashSet<int> { 2, 5 }, new Range(2, 6).EndPoints());
            Assert.AreEqual(new HashSet<int> { 2, 6 }, new Range(2, 6).EndPoints());

        }

        [Test]
        public void TestOverlapsRange()
        {
            Assert.IsFalse(new Range(2, 5).OverlapsRange(new Range(7, 10)));
            Assert.IsTrue(new Range(2, 10).OverlapsRange(new Range(3, 5)));
            Assert.IsTrue(new Range(3, 5).OverlapsRange(new Range(3, 5)));
        }

        [Test]
        public void TestEquals()
        {
            Assert.IsTrue(new Range(3, 5).Equals(new Range(3, 5)));
            Assert.IsFalse(new Range(2, 10).Equals(new Range(3, 5)));
        }
    }
}