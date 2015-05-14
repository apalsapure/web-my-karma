using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.ErrorSpace
{
    [Serializable]
    public class HostRuntimeException : InfrastructureException
    {
        public HostRuntimeException() : base() { }

        public HostRuntimeException(string message) : base(message) { }

        public HostRuntimeException(string message, Exception inner) : base(message, inner) { }

        public HostRuntimeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
