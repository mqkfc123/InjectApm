using SkyApm.Abstractions.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Ids
{

    public abstract class DistributedTraceId : IEquatable<DistributedTraceId>
    {
        private readonly ID _id;

        protected DistributedTraceId(ID id)
        {
            _id = id;
        }

        protected DistributedTraceId(string id)
        {
            _id = new ID(id);
        }

        public string Encode => _id?.Encode;

        public UniqueIdRequest ToUniqueId() => _id?.Transform();

        public bool Equals(DistributedTraceId other)
        {
            if (other == null)
                return false;
            return _id?.Equals(other._id) ?? other._id == null;
        }

        public override bool Equals(object obj)
        {
            var id = obj as DistributedTraceId;
            return Equals(id);
        }

        public override int GetHashCode() => _id != null ? _id.GetHashCode() : 0;

        public override string ToString() => _id?.ToString();
    }
}
