using SkyApm.Abstractions.Context.Ids;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context
{

    public interface IContextCarrier
    {

        DistributedTraceId DistributedTraceId { get; }

        int EntryApplicationInstanceId { get; set; }

        string EntryOperationName { get; set; }

        int EntryOperationId { get; set; }

        int ParentApplicationInstanceId { get; set; }

        string ParentOperationName { get; set; }

        int ParentOperationId { get; set; }

        string PeerHost { get; set; }

        int PeerId { get; set; }

        int SpanId { get; set; }

        ID TraceSegmentId { get; set; }

        bool IsValid { get; }

        IContextCarrier Deserialize(string text);

        string Serialize();

        CarrierItem Items { get; }

        void SetDistributedTraceIds(IEnumerable<DistributedTraceId> distributedTraceIds);
    }
}
