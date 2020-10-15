using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Common
{
    public struct NullableValue
    {
        public static readonly NullableValue Null = new NullableValue(0);

        private const int NULL_VALUE = 0;

        public int Value { get; }

        public NullableValue(int value)
        {
            Value = value;
        }

        public bool HasValue => Value != NULL_VALUE;
    }
}
