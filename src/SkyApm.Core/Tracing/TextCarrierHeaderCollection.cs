using SkyApm.Abstractions.Tracing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Tracing
{

    public class TextCarrierHeaderCollection : ICarrierHeaderCollection
    {
        private readonly IDictionary<string, string> _headers;

        public TextCarrierHeaderCollection(IEnumerable<KeyValuePair<string, string>> headers)
        {
            _headers = new Dictionary<string, string>();
            foreach (var header in headers)
            {
                _headers[header.Key] = header.Value;
            }
        }

        public TextCarrierHeaderCollection(IDictionary<string, string> headers)
        {
            _headers = headers;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _headers.GetEnumerator();
        }

        public void Add(string key, string value)
        {
            _headers[key] = value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _headers.GetEnumerator();
        }
    }
}
