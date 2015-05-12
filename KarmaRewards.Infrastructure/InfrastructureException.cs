using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KarmaRewards.Infrastructure
{
    [Serializable]
    public abstract class InfrastructureException : Exception
    {
        protected InfrastructureException() : base() { }

        protected InfrastructureException(string message) : base(message) { }

        protected InfrastructureException(string message, Exception inner) : base(message, inner) { }

        protected InfrastructureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public static class ExceptionExtension
    {
        public static string GetLogMessage(this Exception exception)
        {
            var sb = new StringBuilder();
            sb.AppendLine("\n\rTimeStamp: " + DateTime.UtcNow.ToString("o"));
            sb.AppendLine("\\n\rType: " + exception.GetType().Name);
            sb.AppendLine("\n\rMessage: " + exception.Message);
            sb.AppendLine("\n\rStack Trace: " + exception.StackTrace);
            return sb.ToString();
        }
    }
}
