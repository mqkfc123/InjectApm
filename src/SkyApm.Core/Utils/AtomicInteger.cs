using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyApm.Core.Utils
{

    public class AtomicInteger
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set { Interlocked.Exchange(ref _value, value); }
        }

        public AtomicInteger()
            : this(0)
        {
        }

        public AtomicInteger(int defaultValue)
        {
            _value = defaultValue;
        }

        public int Increment()
        {
            Interlocked.Increment(ref _value);
            return _value;
        }

        public int Decrement()
        {
            Interlocked.Decrement(ref _value);
            return _value;
        }

        public int Add(int value)
        {
            AddInternal(value);
            return _value;
        }

        private void AddInternal(int value)
        {
            Interlocked.Add(ref _value, value);
        }

        public override bool Equals(object obj)
        {
            if (obj is AtomicInteger)
            {
                return ((AtomicInteger)obj)._value == _value;
            }
            else if (obj is int)
            {
                return Convert.ToInt32(obj) == _value;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static AtomicInteger operator +(AtomicInteger atomicInteger, int value)
        {
            atomicInteger.AddInternal(value);
            return atomicInteger;
        }

        public static AtomicInteger operator +(int value, AtomicInteger atomicInteger)
        {
            atomicInteger.AddInternal(value);
            return atomicInteger;
        }

        public static AtomicInteger operator -(AtomicInteger atomicInteger, int value)
        {
            atomicInteger.AddInternal(-value);
            return atomicInteger;
        }

        public static AtomicInteger operator -(int value, AtomicInteger atomicInteger)
        {
            atomicInteger.AddInternal(-value);
            return atomicInteger;
        }

        public static implicit operator AtomicInteger(int value)
        {
            return new AtomicInteger(value);
        }

        public static implicit operator int(AtomicInteger atomicInteger)
        {
            return atomicInteger._value;
        }

        public static bool operator ==(AtomicInteger atomicInteger, int value)
        {
            return atomicInteger._value == value;
        }

        public static bool operator !=(AtomicInteger atomicInteger, int value)
        {
            return !(atomicInteger == value);
        }

        public static bool operator ==(int value, AtomicInteger atomicInteger)
        {
            return atomicInteger._value == value;
        }

        public static bool operator !=(int value, AtomicInteger atomicInteger)
        {
            return !(value == atomicInteger);
        }
    }
}
