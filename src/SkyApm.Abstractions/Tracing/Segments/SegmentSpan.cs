using SkyApm.Abstractions.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Tracing.Segments
{
    public class SegmentSpan
    {
        public int SpanId { get; } = 0;

        public int ParentSpanId { get; } = -1;

        public long StartTime { get; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public long EndTime { get; private set; }

        public StringOrIntValue OperationName { get; }

        public StringOrIntValue Peer { get; set; }

        public SpanType SpanType { get; }

        public SpanLayer SpanLayer { get; set; }

        /// <summary>Limiting values. Please see <see cref="Components" /> or see <seealso href="https://github.com/apache/skywalking/blob/master/oap-server/server-bootstrap/src/main/resources/component-libraries.yml"/></summary>
        public StringOrIntValue Component { get; set; }
        public bool IsError { get; set; }
        public TagCollection Tags { get; } = new TagCollection();

        public LogCollection Logs { get; } = new LogCollection();

        public SegmentSpan(string operationName, SpanType spanType)
        {
            OperationName = new StringOrIntValue(operationName);
            SpanType = spanType;
        }

        public SegmentSpan AddTag(string key, string value)
        {
            Tags.AddTag(key, value);
            return this;
        }

        public SegmentSpan AddTag(string key, long value)
        {
            Tags.AddTag(key, value.ToString());
            return this;
        }

        public SegmentSpan AddTag(string key, bool value)
        {
            Tags.AddTag(key, value.ToString());
            return this;
        }

        public void AddLog(params LogEvent[] events)
        {
            var log = new SpanLog(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), events);
            Logs.AddLog(log);
        }

        public void Finish()
        {
            EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }

    public class TagCollection : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly Dictionary<string, string> tags = new Dictionary<string, string>();

        internal void AddTag(string key, string value)
        {
            tags[key] = value;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return tags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tags.GetEnumerator();
        }
    }

    public enum SpanType
    {
        Entry = 0,
        Exit = 1,
        Local = 2
    }

    public enum SpanLayer
    {
        DB = 1,
        RPC_FRAMEWORK = 2,
        HTTP = 3,
        MQ = 4,
        CACHE = 5
    }

    public class LogCollection : IEnumerable<SpanLog>
    {
        private readonly List<SpanLog> _logs = new List<SpanLog>();

        internal void AddLog(SpanLog log)
        {
            _logs.Add(log);
        }

        public IEnumerator<SpanLog> GetEnumerator()
        {
            return _logs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _logs.GetEnumerator();
        }
    }

    public class SpanLog
    {
        private static readonly Dictionary<string, string> Empty = new Dictionary<string, string>();
        public long Timestamp { get; }

        public IReadOnlyDictionary<string, string> Data { get; }

        public SpanLog(long timestamp, params LogEvent[] events)
        {
            Timestamp = timestamp;
            Data = events?.ToDictionary(x => x.Key, x => x.Value) ?? Empty;
        }
    }

    public class LogEvent
    {
        public string Key { get; }

        public string Value { get; }

        public LogEvent(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public static LogEvent Event(string value)
        {
            return new LogEvent("event", value);
        }

        public static LogEvent Message(string value)
        {
            return new LogEvent("message", value);
        }

        public static LogEvent ErrorKind(string value)
        {
            return new LogEvent("error.kind", value);
        }

        public static LogEvent ErrorStack(string value)
        {
            return new LogEvent("stack", value);
        }
    }
}
