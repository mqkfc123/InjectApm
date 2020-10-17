
namespace SkyApm.Abstractions.Common
{

    public struct StringOrIntValue
    {
        private readonly int _intValue;
        private readonly string _stringValue;

        public StringOrIntValue(int value)
        {
            _intValue = value;
            _stringValue = null;
        }

        public bool HasValue => HasIntValue || HasStringValue;

        public bool HasIntValue => _intValue != 0;

        public bool HasStringValue => _stringValue != null;

        public StringOrIntValue(string value)
        {
            _intValue = 0;
            _stringValue = value;
        }

        public StringOrIntValue(int intValue, string stringValue)
        {
            _intValue = intValue;
            _stringValue = stringValue;
        }

        public int GetIntValue() => _intValue;

        public string GetStringValue() => _stringValue;

        //public (string, int) GetValue()
        //{
        //    return (_stringValue, _intValue);
        //}

        public override string ToString()
        {
            if (HasIntValue) return _intValue.ToString();
            return _stringValue;
        }

        public static implicit operator StringOrIntValue(string value) => new StringOrIntValue(value);
        public static implicit operator StringOrIntValue(int value) => new StringOrIntValue(value);
    }
}
