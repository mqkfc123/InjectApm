using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Tracing.Segments;
using SkyApm.Core.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CInject.SampleWinform
{
    public class Worker
    {
        private readonly ITracingContext _tracingContext;

        public Worker(ITracingContext tracingContext)
        {
            _tracingContext = tracingContext;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var context = _tracingContext.CreateEntrySegmentContext(nameof(ExecuteAsync), new TextCarrierHeaderCollection(new Dictionary<string, string>()));

            await Task.Delay(1000, stoppingToken);

            context.Span.AddTag("新节点", "测试");
            context.Span.AddLog(LogEvent.Message($"Worker running at: {DateTime.Now}"));

            _tracingContext.Release(context);
             
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    var context = _tracingContext.CreateEntrySegmentContext(nameof(ExecuteAsync), new TextCarrierHeaderCollection(new Dictionary<string, string>()));

            //    await Task.Delay(1000, stoppingToken);

            //    context.Span.AddTag("新节点", "测试");
            //    context.Span.AddLog(LogEvent.Message($"Worker running at: {DateTime.Now}"));

            //    _tracingContext.Release(context);
            //}
        }
    }
}
