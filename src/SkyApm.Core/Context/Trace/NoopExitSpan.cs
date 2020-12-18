using SkyApm.Abstractions.Context.Trace;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context.Trace
{

    public class NoopExitSpan : NoopSpan, IWithPeerInfo
    {
        public int PeerId => peerId;

        public string Peer => peer;

        private String peer;
        private int peerId;

        public NoopExitSpan(int peerId)
        {
            this.peerId = peerId;
        }

        public NoopExitSpan(String peer)
        {
            this.peer = peer;
        }

        public override bool IsExit { get; } = true;
    }
}
