using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Tracing
{
    public struct UniqueId : IEquatable<UniqueId>
    {
        public long Part1 { get; }

        public long Part2 { get; }

        public long Part3 { get; }

        public UniqueId(long part1, long part2, long part3)
        {
            Part1 = part1;
            Part2 = part2;
            Part3 = part3;
        }

        public override string ToString() => $"{Part1}.{Part2}.{Part3}";

        public bool Equals(UniqueId other) =>
            Part1 == other.Part1 && Part2 == other.Part2 && Part3 == other.Part3;

        public override bool Equals(object obj)
        {
            if (obj is UniqueId)
            {
                var other = (UniqueId)obj;
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Part1.GetHashCode();
                hashCode = (hashCode * 397) ^ Part2.GetHashCode();
                hashCode = (hashCode * 397) ^ Part3.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(UniqueId left, UniqueId right) => left.Equals(right);

        public static bool operator !=(UniqueId left, UniqueId right) => !left.Equals(right);
    }
}
