using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Trace
{
    public interface IWithPeerInfo
    {
        int PeerId { get; }

        string Peer { get; }
    }
}
