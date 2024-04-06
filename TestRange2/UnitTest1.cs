using System.Collections.Generic;


namespace TestRange2
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestContains_ContainsCorrectValues()
        {
            Range range = new Range("[2,6)");
            Assert.IsTrue(range.Contains(2)); // Debe devolver true, ya que 2 está dentro de [2,6)
            Assert.IsFalse(range.Contains(4)); // Debe devolver true, ya que 4 está dentro de [2,6)

        }

        [Test]
        public void TestGetAllPoints_ReturnsCorrectPoints()
        {
            Range range = new Range("[2,6)");
            var allPoints = range.GetAllPoints();
            Assert.AreEqual(new HashSet<int> { 2, 3, 4, 5 }, allPoints); // Debe devolver un conjunto que contiene {2, 3, 4, 5} para el rango [2,6)
        }

        [Test]
        public void TestContainsRange_ChecksIfRangeContainsAnotherRange()
        {
            Range range2 = new Range("[2,5)");
            Range range3 = new Range("[3,5)");
            Range range4 = new Range("[7,10)");

            Assert.IsFalse(range2.ContainsRange(range4)); // Debe devolver false, ya que [2,5) no contiene [7,10)
            Assert.IsFalse(range2.ContainsRange(range3)); // Debe devolver false, ya que [2,5) no contiene [3,5)
        }

        [Test]
        public void TestEndPoints_ReturnsCorrectEndPoints()
        {
            Assert.AreEqual(new HashSet<int> { 2, 5 }, new Range("[2,6)").EndPoints()); // Debe devolver un conjunto que contiene {2, 5} para el rango [2,6)
            Assert.AreEqual(new HashSet<int> { 2, 6 }, new Range("[2,6]").EndPoints()); // Debe devolver un conjunto que contiene {2, 6} para el rango [2,6]
        }

        [Test]
        public void TestOverlapsRange_ChecksIfRangesOverlap()
        {
            Assert.IsFalse(new Range("[2,5)").OverlapsRange(new Range("[7,10)"))); // [2,5) doesn’t overlap with [7,10)
            Assert.IsTrue(new Range("[2,10)").OverlapsRange(new Range("[3,5]"))); // [2,10) overlaps with [3,5)
            Assert.IsTrue(new Range("[3,5]").OverlapsRange(new Range("[3,5]"))); // [3,5) overlaps with [3,5)
            Assert.IsTrue(new Range("[2,5)").OverlapsRange(new Range("[3,10)"))); // [2,5) overlaps with [3,10)
            Assert.IsTrue(new Range("[3,5]").OverlapsRange(new Range("[2,10)"))); // [3,5) overlaps with [2,10)
        }

        [Test]
        public void TestEquals_ChecksIfRangesAreEqual()
        {
            Assert.IsTrue(new Range("[3,5]").Equals(new Range("[3,5]"))); // [3,5) equals [3,5)
            Assert.IsFalse(new Range("[2,10)").Equals(new Range("[3,5]"))); // [2,10) neq [3,5)
            Assert.IsFalse(new Range("[2,5)").Equals(new Range("[3,10)"))); // [2,5) neq [3,10)
            Assert.IsFalse(new Range("[3,5]").Equals(new Range("[2,10)"))); // [3,5) neq [2,10)
        }

    }

    public class Range
    {
        private int start;
        private int end;
        private bool isStartInclusive;
        private bool isEndInclusive;

        public Range(string rangeString)
        {
            if (string.IsNullOrEmpty(rangeString))
                throw new ArgumentException("Range string cannot be null or empty.");

            // Remove any white spaces and trim brackets
            rangeString = rangeString.Trim();

            // Determine inclusivity based on the presence of '[' or ']'
            isStartInclusive = rangeString.StartsWith("[");
            isEndInclusive = rangeString.EndsWith("]") || rangeString.EndsWith(")");

            // Remove brackets and split by comma
            rangeString = rangeString.Trim('[', ']', '(', ')');
            string[] parts = rangeString.Split(',');

            if (parts.Length != 2)
                throw new ArgumentException("Invalid range string format.");

            // Parse the start and end values
            if (!int.TryParse(parts[0], out start))
                throw new ArgumentException("Invalid start value.");

            if (!int.TryParse(parts[1], out end))
                throw new ArgumentException("Invalid end value.");
        }

        public bool Contains(int value)
        {
            if (isStartInclusive && isEndInclusive)
            {
                return value >= start && value <= end;
            }
            else if (isStartInclusive && !isEndInclusive)
            {
                return value >= start && value < end; // Solo negamos el endInclusive
            }
            else if (!isStartInclusive && isEndInclusive)
            {
                return value > start && value <= end; // Solo negamos el startInclusive
            }
            else // Cuando no es ni al inicio ni al final
            {
                return value > start && value < end;
            }
        }

        public HashSet<int> GetAllPoints()
        {
            HashSet<int> points = new HashSet<int>();

            if (isStartInclusive)
            {
                points.Add(start); // Siempre incluimos el inicio si es inclusivo
            }

            for (int i = start + (isStartInclusive ? 1 : 0); i < end; i++) // Ajuste en el for
            {
                points.Add(i);
            }

            if (isEndInclusive)
            {
                points.Add(end); // Incluimos el final solo si es inclusivo
            }

            return points;
        }

        public bool ContainsRange(Range other)
        {
            if (isStartInclusive && other.isEndInclusive)
            {
                return start <= other.start && end >= other.end; // Caso [] y ()
            }
            else if (isStartInclusive && !other.isEndInclusive)
            {
                return start <= other.start && end > other.end; // Caso [] y )
            }
            else if (!isStartInclusive && other.isEndInclusive)
            {
                return start < other.start && end >= other.end; // Caso (] y []
            }
            else // Cuando ninguno de los extremos es inclusivo o ambos son exclusivos (] y ()
            {
                return start < other.start && end < other.end;
            }
        }

        public HashSet<int> EndPoints()
        {
            HashSet<int> endPoints = new HashSet<int>();
            if (isStartInclusive)
            {
                endPoints.Add(start);
            }
            if (isEndInclusive)
            {
                endPoints.Add(end);
            }
            return endPoints;
        }

        public bool OverlapsRange(Range other)
        {
            return start < other.end && end > other.start;
        }

        public bool Equals(Range other)
        {
            return start == other.start && end == other.end && isStartInclusive == other.isStartInclusive && isEndInclusive == other.isEndInclusive;
        }
    }

}