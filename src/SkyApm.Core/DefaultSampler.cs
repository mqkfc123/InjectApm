using SkyApm.Abstractions;
using SkyApm.Core.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SkyApm.Core
{

    public class DefaultSampler : ISampler
    {
        public static DefaultSampler Instance { get; } = new DefaultSampler();

        private readonly AtomicInteger _idx = new AtomicInteger();

        private int _samplePer3Secs;
        private bool _sample_on;

        public bool Sampled()
        {
            if (!_sample_on)
            {
                return true;
            }

            return _idx.Increment() < _samplePer3Secs;
        }

        public void ForceSampled()
        {
            if (_sample_on)
            {
                _idx.Increment();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void SetSamplePer3Secs(int samplePer3Secs)
        {
            _samplePer3Secs = samplePer3Secs;
            _sample_on = samplePer3Secs > -1;
        }

        internal void Reset()
        {
            _idx.Value = 0;
        }
    }
}
