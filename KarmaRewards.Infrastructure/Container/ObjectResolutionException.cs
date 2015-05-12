using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.Container
{
    [Serializable]
    public class ObjectResolutionException : InfrastructureException
    {
        public ObjectResolutionException() : base() { }

        public ObjectResolutionException(string message) : base(message) { }

        public ObjectResolutionException(string message, Exception inner) : base(message, inner) { }

        public ObjectResolutionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
