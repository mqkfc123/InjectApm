using SkyWalking.NetworkProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CInject.SampleWinform.Transport
{


    public enum Reference
    {
        CrossProcess = 0,
        CrossThread = 1
    }

    public class SegmentContextMapper
    {

        public SegmentRequest Map(int serviceId, int serviceInstanceId, string value)
        {
            var iniqueIds = new UniqueIdRequest
            {
                Part1 = serviceId,
                Part2 = serviceInstanceId,
                Part3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            var segmentRequest = new SegmentRequest
            {
                UniqueIds = new[]
                {
                   iniqueIds
                }
            };

            var segmentObjectRequest = new SegmentObjectRequest
            {
                SegmentId = iniqueIds,
                ServiceId = serviceId,
                ServiceInstanceId = serviceInstanceId,
            };
            segmentRequest.Segment = segmentObjectRequest;


            var startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Thread.Sleep(500);

            var endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var span = new SpanRequest
            {
                SpanId = 0,
                ParentSpanId = -1,
                OperationName = "sample_client_" + value, 
                StartTime= startTime,
                EndTime = endTime,
                SpanType = (int)SpanType.Entry,
                SpanLayer = (int)SpanLayer.Rpcframework,
                IsError = false,
                Peer = "10.16.2.113", //httpContext.Connection.RemoteIpAddress
                Component = "GRPC" //GRPC HttpClient
            };

            //span.References.Add(new SegmentReferenceRequest
            //{
            //    ParentSegmentId = segmentObjectRequest.SegmentId,
            //    ParentServiceInstanceId = segmentObjectRequest.ServiceInstanceId,
            //    ParentSpanId = 0,
            //    ParentEndpointName = span.OperationName,
            //    EntryServiceInstanceId = serviceInstanceId,
            //    EntryEndpointName = span.OperationName,
            //    NetworkAddress = "10.16.2.113",
            //    RefType = (int)Reference.CrossProcess
            //});


            span.Tags.Add(new KeyValuePair<string, string>(Tags.URL, "api/index"));
            span.Tags.Add(new KeyValuePair<string, string>(Tags.PATH, "127.0.0.1"));
            span.Tags.Add(new KeyValuePair<string, string>(Tags.HTTP_METHOD, "btnChangeValue_Click"));
            span.Tags.Add(new KeyValuePair<string, string>(Tags.STATUS_CODE, "200"));

            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            var logData = new LogDataRequest { Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() };
            logData.Data.Add(new KeyValuePair<string, string>("日志1", "data.Value1"));
            logData.Data.Add(new KeyValuePair<string, string>("日志2", "data.Value2"));
            span.Logs.Add(logData);

            segmentObjectRequest.Spans.Add(span);


            var span2 = new SpanRequest
            {
                SpanId = 1,
                ParentSpanId = 0,
                OperationName = "sample_client_c" + value,
                StartTime = startTime,
                EndTime = endTime,
                SpanType = (int)SpanType.Local,
                SpanLayer = (int)SpanLayer.Rpcframework,
                IsError = false,
                Peer = "10.16.2.113", //httpContext.Connection.RemoteIpAddress
                Component = "GRPC" //GRPC HttpClient
            };

            segmentObjectRequest.Spans.Add(span2);
            return segmentRequest;

        }

    }
}
