using System.Collections.Generic;
using System;

namespace RangoTest
{
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
