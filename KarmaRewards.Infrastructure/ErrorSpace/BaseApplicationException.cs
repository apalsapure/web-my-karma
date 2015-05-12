using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace KarmaRewards.Infrastructure.ErrorSpace
{
    [Serializable]
    public class BaseApplicationException : Exception
    {
        public BaseApplicationException() : base() { }

        public BaseApplicationException(string message) : base(message) { }

        public BaseApplicationException(string message, Exception inner) : base(message, inner) { }

        public BaseApplicationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public string TransactionId { get; set; }

        public HttpErrorCode ErrorCode { get; set; }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            if (this.TransactionId != null)
                info.AddValue("TransactionId", this.TransactionId);
            info.AddValue("ErrorCode", this.ErrorCode.ToString());

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }
    }
}
