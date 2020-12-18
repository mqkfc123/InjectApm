using SkyApm.Abstractions.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Trace
{

    public class LogDataEntity
    {
        private readonly long _timestamp;
        private readonly Dictionary<string, string> _logs;

        private LogDataEntity(long timestamp, Dictionary<string, string> logs)
        {
            _timestamp = timestamp;
            _logs = logs;
        }

        public IDictionary<string, string> Logs => new Dictionary<string, string>(_logs);

        public class Builder
        {
            private readonly Dictionary<string, string> _logs;

            public Builder()
            {
                _logs = new Dictionary<string, string>();
            }

            public Builder Add(IDictionary<string, string> fields)
            {
                foreach (var field in fields)
                    _logs.Add(field.Key, field.Value);
                return this;
            }

            public Builder Add(string key, string value)
            {
                _logs.Add(key, value);
                return this;
            }

            public LogDataEntity Build(long timestamp)
            {
                return new LogDataEntity(timestamp, _logs);
            }
        }

        public LogDataRequest Transform()
        {
            var logMessage = new LogDataRequest();
            logMessage.Timestamp = _timestamp;
            foreach (var log in _logs)
            {
                logMessage.Data.Add(new KeyValuePair<string, string>(log.Key, log.Value));
            }

            return logMessage;
        }
    }
}
